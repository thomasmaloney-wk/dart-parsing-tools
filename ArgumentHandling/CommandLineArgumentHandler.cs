namespace DartSharp.ArgumentHandling
{
    class CommandLineArgumentHandler
    {
        private List<string> GetFilesFromDirectory(string directory)
        {
            return Directory.GetFiles(directory, "*.dart", SearchOption.AllDirectories).ToList();
        }

        public CommandLineArgumentParsePayload ParseArguments(string[] args)
        {
            var files = new List<string>();
            var genericFlagLookupMap = FlagRegistry.GenericFlags.ToDictionary(k => k.Flag, v => v);
            var processorFlagLookupMap = FlagRegistry.ProcessorFlags.ToDictionary(k => k.Flag, v => v);
            var processorTypeLookupMap = FlagRegistry.ProcessorFlags.ToDictionary(k => (k as IProcessorArgumentFlag).ProcessorType, v => v);

            var payload = new CommandLineArgumentParsePayload();

            if (args.Length == 0)
            {
                payload.Errors.Add("Error: No arguments provided.");
                return payload;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var currentArg = args[i];

                if (genericFlagLookupMap.TryGetValue(currentArg, out GenericArgumentFlag? genericFlag))
                {
                    var flagParams = new List<string>();
                    var paramCount = genericFlag.ParameterCount;
                    if (paramCount > 0)
                    {
                        // Sanity check. Make sure the correct number of arguments are supplied to the flag
                        if (i + paramCount >= args.Length)
                        {
                            // Todo: implement better error handling
                            payload.Errors.Add($"Error: '{genericFlag.Flag}' requires {paramCount} args but was only given {args.Length - i} args.");
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

                if (processorFlagLookupMap.TryGetValue(currentArg, out ArgumentFlag? pFlag))
                {
                    // Attempt to cast to ProcessorArgumentFlag<TProcessor> dynamically

                    // This in theory should never happen since pFlag must be an IProcessorArgumentFlag
                    // to exist in FlagRegistry.ProcessorFlags.
                    // But I'll leave this check in until I am comfortable with the command argument handling
                    // rewrite.
                    if (pFlag is not IProcessorArgumentFlag processorFlag)
                    {
                        payload.Errors.Add($"Error: Flag '{pFlag.Flag}' not associated with processor.");
                        continue;
                    }

                    if (!payload.TrySetProcessorFactory(processorFlag.CreateProcessor))
                    {
                        payload.Errors.Add($"Error: cannot set flag '{pFlag.Flag}' as another process flag has been set already.");
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
                payload.Errors.Add($"Unrecognized command `{currentArg}");
                return payload;
            }

            if (payload.GenericFlags.TryGetValue("--dir", out IEnumerable<string>? value))
            {
                files = files.Union(GetFilesFromDirectory(value.First())).ToList();
            }

            payload.Files.AddRange(files);

            return payload;
        }
    }
}