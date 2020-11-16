﻿using System;
using System.Collections.Generic;

namespace GRUnlocker {
    public class InputHandler {

        private List<OptionCore> options = new List<OptionCore>();

        public InputHandler() {
            // print intro
            Console.WriteLine("════Ghostrunner Unlocker [by DmgVol]════");
            Console.WriteLine($"Save path: {(Config.getInstance().IsLocalSavePath ? "local(same directory)" : "remote(from config)")}\n");

            if(CheckSaveExists(false)) {
                // ==unlocking==
                options.Add(new Option_UnlockAllCollectibles());
                options.Add(new Option_UnlockAllLevels());
                options.Add(new Option_UnlockEverything());
                options.Add(new Option_UnlockUpToLevel());
                options.Add(new Option_NewGameCollectibles());
                options.Add(new Option_NewGameSword());
                options.Add(new Option_ReplaceSelectedSword());
                // ==resetting==
                options.Add(new Option_ResetLevelDetails());
                options.Add(new Option_ResetCollectibles());

                // ==game related==
                if(!Config.getInstance().IsLocalSavePath)
                    options.Add(new Option_ToggleIntros());
                options.Add(new Option_RemoveKevin());
            }

            // == GRUnlocker related== 
            // this option does not require .sav file nor valid config file
            options.Add(new Option_CreateConfig());
        }

        public void Handle() {
            Console.WriteLine("─────────────────────────────────");
            Console.WriteLine("───Options (type in the digit)───");
            for(int i = 0; i < options.Count; i++) {
                Console.WriteLine($"{i + 1} - {options[i].desc}");
            }
            Console.WriteLine("─────────────────────────────────");

            // read input or args if any
            string input = "";
            if(Program.ARGS != null && Program.ARGS.Length == 1) {
                input += Program.ARGS[0][0];
                Console.WriteLine("Using args: " + input);
            } else {
                Console.Write("\nOption:\t");
                input = Console.ReadLine();
            }

            // valid?
            bool successFlag = true;
            int optionNumber;
            if(Int32.TryParse(input, out optionNumber) && optionNumber > 0 && optionNumber < options.Count + 1) {
                int i = optionNumber - 1;
                // special case - unlock up to level, requires more input
                if(options[i] is Option_UnlockUpToLevel) {
                    Console.WriteLine("Up to which level to unlock? (1-16)");
                    Console.WriteLine("─────────────────────────────────");
                    Console.Write(">");
                    optionNumber = GetOptionNumber(0, 17);
                    successFlag = options[i].Patch(optionNumber);
                } else if(options[i] is Option_ReplaceSelectedSword || options[i] is Option_NewGameSword) {
                    // special case - replace selected sword, requires more input
                    Console.WriteLine("Select your sword: (no pre-order swords)");
                    Console.WriteLine(SwordData.DisplaySwordNames());
                    Console.WriteLine("─────────────────────────────────");
                    Console.Write("Sword number: ");
                    optionNumber = GetOptionNumber(-1, 15);
                    successFlag = options[i].Patch(optionNumber);
                } else {
                    // patch and return result
                    successFlag = options[i].Patch();
                }
            } else {
                Console.WriteLine("Error: invalid input.");
                ExitProgram();
            }

            // print results
            if(successFlag) 
                Console.WriteLine("Patched successfully!");
            else
                Console.WriteLine("Error: failed to patch!\n-Something went wrong, missing permissions/files or corrupted .sav file");

            ExitProgram();
        }

        private int GetOptionNumber(int min, int max) {
            int optionNumber = -1;
            string input = Console.ReadLine();
            if(Int32.TryParse(input, out optionNumber) && optionNumber > min && optionNumber < max) {
                return optionNumber;
            }

            Console.WriteLine("Error: invalid input.");
            ExitProgram();
            return -1;
        }

        public static bool CheckSaveExists(bool exitIfMissing = true) {
            if(!Unlocker.FileExists()) {
                if(Config.getInstance().IsLocalSavePath)
                    Console.WriteLine("Error: Missing .sav file!\n-Make sure the save file(Ghostrunner.sav) is in the same directory as the executable");
                else
                    Console.WriteLine("Error: Missing .sav file!\n-Save file(Ghostrunner.sav) is missing from the given path (config file)");

                if(exitIfMissing)  ExitProgram();
                return false;
            }
            return true;
        }
        public static void ExitProgram() {
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
