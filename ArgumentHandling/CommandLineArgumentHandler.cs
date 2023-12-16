namespace DartSharp.ArgumentHandling
{
    class CommandLineArgumentHandler
    {
        private static readonly CommandLineArgumentParsePayload payload = new();
        private static List<string> GetFilesFromDirectory(string directory)
        {
            try
            {
                return Directory.GetFiles(directory, "*.dart", SearchOption.AllDirectories).ToList();
            }
            catch (DirectoryNotFoundException)
            {
                payload.Errors.Add($"Error: Directory {directory} not found.");
                return new();
            }
        }

        public static CommandLineArgumentParsePayload ParseArguments(string[] args)
        {
            var files = new List<string>();

            if (args.Length == 0)
            {
                payload.Errors.Add("Error: No arguments provided.");
                return payload;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var currentArg = args[i];

                if (FlagRegistry.GenericFlagByFlagName.TryGetValue(currentArg, out GenericArgumentFlag? genericFlag))
                {
                    var flagParams = new List<string>();
                    var paramCount = genericFlag.ParameterCount;
                    if (paramCount > 0)
                    {
                        // Sanity check. Make sure the correct number of arguments are supplied to the flag
                        if (i + paramCount >= args.Length)
                        {
                            // Todo: implement better error handling
                            payload.Errors.Add($"Error: '{genericFlag.Flag}' requires {paramCount} args but was only given {args.Length - i - 1} args.");
                            continue;
                        }

                        while (paramCount-- > 0)
                        {
                            flagParams.Add(args[++i]);
                        }
                    }
                    payload.GenericFlags[genericFlag.Flag] = flagParams;
                    continue;
                }

                if (FlagRegistry.ProcessorFlagByFlagName.TryGetValue(currentArg, out ArgumentFlag? argFlag))
                {
                    // Attempt to cast to ProcessorArgumentFlag<TProcessor> dynamically

                    // This in theory should never happen since argFlag must be an IProcessorArgumentFlag
                    // to exist in FlagRegistry.ProcessorFlags.
                    // But I'll leave this check in until I am comfortable with the command argument handling
                    // rewrite.
                    if (argFlag is not IProcessorArgumentFlag processorFlag)
                    {
                        payload.Errors.Add($"Error: Flag '{argFlag.Flag}' not associated with processor.");
                        continue;
                    }

                    if (!payload.TrySetProcessorFactory(processorFlag.CreateProcessor))
                    {
                        payload.Errors.Add($"Error: cannot set flag '{argFlag.Flag}' as another process flag has been set already.");
                        continue; ;
                    }
                    continue;
                }

                if (currentArg.EndsWith(".dart"))
                {
                    files.Add(currentArg);
                    continue;
                }

                // If we reach here, an invalid file or argument has been passed in
                payload.Errors.Add($"Unrecognized command `{currentArg}`");
                return payload;
            }

            if (payload.GenericFlags.TryGetValue("--dir", out IEnumerable<string>? value))
            {
                files = files.Union(GetFilesFromDirectory(value.First())).ToList();
            }

            payload.Files.AddRange(files);

            if (!payload.IsProcessorFlagSet)
            {
                payload.Errors.Add("Error: Program requires processor flag");
            }

            return payload;
        }
    }
}