using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using SoulsFormats;
using WeakSpotInator;
using NLua;
using HKLib.hk2018;

namespace Limbs
{

    internal class Program
    {
        private static HKXPWV hkxpwvLua = new HKXPWV();

        private static void readHKXPWVAndOutputFiles(String chrbndPath)
        {

            BND4 chrbnd = BND4.Read(chrbndPath);
            var limbFile = new BinderFile();
            try
            {
                limbFile = chrbnd.Files.Where(
                    x => x.Name.Contains("hkxpwv")
                )
                .First();
            }
            catch
            {
                return; //returns if no hkxpwv
            }

            HKXPWV limbs = new HKXPWV();

            limbs.Game = HKXPWV.GameType.DS3;

            limbs = HKXPWV.Read(
                limbFile.Bytes
            );

            

            // Define the regex pattern to extract the desired characters
            string pattern = @"([^\\]+)\.chrbnd\.dcx$";

            // Match the pattern against the file path
            Match match = Regex.Match(chrbndPath, pattern);
            string character = "";
            if (match.Success)
            {
                // Extract the matched group (characters before ".chrbnd.dcx")
                character = match.Groups[1].Value;

                Console.WriteLine("Character: " + character);
            }
            else
            {
                Console.WriteLine("File path doesn't match the expected pattern (cXXXX.chrbnd.dcx)");
            }

            Weakspot weakspot = new Weakspot(limbs, chrbndPath);
            //HKXPWV.RagdollBoneEntry nutsToHeadshot = new HKXPWV.RagdollBoneEntry();
            //nutsToHeadshot.DisableHit = false;
            //nutsToHeadshot.DamageAnimID = -1;
            //nutsToHeadshot.DisableCollision = false;
            //nutsToHeadshot.NPCPartDamageGroup = 6;
            //nutsToHeadshot.NPCPartGroupIndex = 28;
            //nutsToHeadshot.RagdollParamID = -1;
            //nutsToHeadshot.UnknownBB = -1;
            /*
             * weakspot.editMapping(
                "R_Foot",
                "Ragdoll_R_Foot001",
                nutsToHeadshot
                )
             * weakspot.writeToFiles();
             */


            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string filePathStd = @".\outputs\" + character + ".json";

            // Write the serialized JSON to a file
            File.WriteAllText(
                filePathStd,
                JsonSerializer.Serialize(limbs, options)
            );

            string filePathWkPt = @".\outputsWeakPoints\" + character + ".json";

            // Write the serialized JSON to a file
            File.WriteAllText(
                filePathWkPt,
                JsonSerializer.Serialize(weakspot.boneSpots, options)
            );


        }

        private static void outputHKXPWV(String directory) {

            try
            {
                string[] files = Directory.GetFiles(directory, "*.chrbnd.dcx");

                if (files.Length > 0)
                {
                    Console.WriteLine("Files with .chrbnd.dcx extension:");

                    foreach (string file in files)
                    {
                        try
                        {
                            readHKXPWVAndOutputFiles(file);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An error occurred: " + e.Message + "\nStack Trace: " + e.StackTrace);
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No files found with .chrbnd.dcx extension.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
            Console.WriteLine("Done! (Enter key to exit.)");
            Console.ReadLine();
        }

        private static void runLuaAtHKXPWV(String luaFile, String chrbndPath) {

            Lua state = new Lua();

            BND4 chrbnd = BND4.Read(chrbndPath);
            var limbFile = new BinderFile();
            try
            {
                limbFile = chrbnd.Files.Where(
                    x => x.Name.Contains("hkxpwv")
                )
                .First();
            }
            catch
            {
                return; //returns if no hkxpwv
            }

            HKXPWV limbs = new HKXPWV();

            limbs.Game = HKXPWV.GameType.DS3;

            limbs = HKXPWV.Read(
                limbFile.Bytes
            );

            // Define the regex pattern to extract the desired characters
            string pattern = @"([^\\]+)\.chrbnd\.dcx$";

            // Match the pattern against the file path
            Match match = Regex.Match(chrbndPath, pattern);
            string character = "";
            if (match.Success)
            {
                // Extract the matched group (characters before ".chrbnd.dcx")
                character = match.Groups[1].Value;

                Console.WriteLine("Character: " + character + " is being run against: " + luaFile + "!");
            }
            else
            {
                Console.WriteLine("File path doesn't match the expected pattern (cXXXX.chrbnd.dcx)");
                return;
            }


            Weakspot weakspot = new Weakspot(limbs, chrbndPath);

            state.LoadCLRPackage();
            state.DoString(@" import ('System')");
            state["characterEdited"] = character;
            state["hkxpwvEdited"] = weakspot;
            Utilities utilities = new Utilities();
            state["utilities"] = utilities;

            String luaFileContent = File.ReadAllText(luaFile);

            Console.WriteLine("Running:\n\n" + luaFileContent + "\n\n");

            state.DoString(luaFileContent);

            Console.WriteLine("Complete.");
        }
        public class Utilities {

            public Utilities() { 
                
            }
            public void print(string thingToPrint) {
                Console.WriteLine(thingToPrint);
            }
        }
        private static void runLuaAtDirectoryOfHKXPWV(String luaFile, String directory) {

            try
            {
                string[] files = Directory.GetFiles(directory, "*.chrbnd.dcx");

                if (files.Length > 0)
                {
                    Console.WriteLine("Files with .chrbnd.dcx extension:");

                    foreach (string file in files)
                    {
                        try
                        {
                            runLuaAtHKXPWV(luaFile, file);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An error occurred: " + e.Message + "\nStack Trace: " + e.StackTrace);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No files found with .chrbnd.dcx extension.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
            Console.WriteLine("Done! (Enter key to exit.)");
            Console.ReadLine();
        }
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("You need to drag a file directory on to me, or a lua file and a file directory on to me!");
                Console.ReadLine();
                return;
            }
            else if (args.Length == 1)
            {
                outputHKXPWV(args[0]);
                return;
            }
            else if (args.Length == 2) {
                String luaFileAt = "";
                String hkxPWVDirAt = "";
                if (args[0].EndsWith(".lua"))
                {
                    luaFileAt = args[0];
                    hkxPWVDirAt = args[1];
                }
                else
                {
                    luaFileAt = args[1];
                    hkxPWVDirAt = args[0];
                }
                runLuaAtDirectoryOfHKXPWV(luaFileAt, hkxPWVDirAt);
                return;
            } else if (args.Length == 3)
            {
                Console.WriteLine("You need to drag a file directory on to me, or a lua file and a file directory on to me!");
                Console.ReadLine();
                return;
            }

        }
    }
}
