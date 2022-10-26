﻿using System;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace MailCheck.Common.Logging
{
    public class RenderedJsonFormatter : ITextFormatter
    {
        readonly JsonValueFormatter _valueFormatter;

        public RenderedJsonFormatter(JsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new JsonValueFormatter(typeTagName: "$type");
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            FormatEvent(logEvent, output, _valueFormatter);
            output.WriteLine();
        }

        public static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (valueFormatter == null) throw new ArgumentNullException(nameof(valueFormatter));

            output.Write("{\"Timestamp\":\"");
            output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));
            output.Write("\",\"Message\":");
            var message = logEvent.MessageTemplate.Render(logEvent.Properties);
            JsonValueFormatter.WriteQuotedJsonString(message, output);
            output.Write(",\"Id\":\"");
            var id = EventIdHash.Compute(logEvent.MessageTemplate.Text);
            output.Write(id.ToString("x8"));
            output.Write('"');

            output.Write(",\"Level\":\"");
            output.Write(logEvent.Level);
            output.Write('\"');

            if (logEvent.Exception != null)
            {
                output.Write(",\"Exception\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            foreach (var property in logEvent.Properties)
            {
                var name = property.Key;
                if (name.Length > 0 && name[0] == '@')
                {
                    name = '@' + name;
                }

                output.Write(',');
                JsonValueFormatter.WriteQuotedJsonString(name, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output);
            }

            output.Write('}');
        }
    }
}
