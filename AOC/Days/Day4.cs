using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC {
    class Day4 {
        public static readonly HashSet<string> VALID_PROPERTIES = new HashSet<string>() { 
            "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" 
        };
        public static void SolveDay4_1() {
            int count = 0;
            StringBuilder passportData = new StringBuilder();
            using (StreamReader reader = new StreamReader("Inputs/day4.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    passportData.Append($"{line} ");
                    if (line.Equals("")) {
                        if (IsValidPassport_Day1(passportData.ToString())) count++;
                        passportData.Clear();
                    }
                }
            }
            Console.WriteLine($"There are {count} valid passports");
        }

        public static void SolveDay4_2() {
            int count = 0;
            StringBuilder passportData = new StringBuilder();
            using (StreamReader reader = new StreamReader("Inputs/day4.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    passportData.Append($"{line} ");
                    if (line.Equals("")) {
                        if (IsValidPassport_Day2(passportData.ToString())) count++;
                        passportData.Clear();
                    }
                }
            }
            Console.WriteLine($"There are {count} valid passports");
        }



        private static bool IsValidPassport_Day1(string passportString) {
            bool hasCid = false;
            string[] passportSplit = passportString.TrimEnd().Split(" ");
            if (passportSplit.Length < 7) return false;
            foreach (string property in passportSplit) {
                string curProp = property.Split(":")[0];
                if (!VALID_PROPERTIES.Contains(curProp)) return false;
                if (curProp.Equals("cid")) hasCid = true;
            }
            return passportSplit.Length > 7 || passportSplit.Length == 7 && !hasCid;
        }

        private static bool IsValidPassport_Day2(string passportString) {
            bool hasCid = false;
            string[] passportSplit = passportString.TrimEnd().Split(" ");
            if (passportSplit.Length < 7) return false;
            foreach (string property in passportSplit) {
                string curProp = property.Split(":")[0];
                if (!VALID_PROPERTIES.Contains(curProp) || !ValidateProp(property)) return false;
                if (curProp.Equals("cid")) hasCid = true;
            }
            return passportSplit.Length > 7 || passportSplit.Length == 7 && !hasCid;
        }

        private static bool ValidateProp(string prop) {
            string[] split = prop.Split(":");
            string regex;
            switch (split[0]) {
                case "byr":
                    regex = "^([1]{1}[9]{1}[2-9]{1}[0-9]{1}|[2]{1}[0]{1}[0]{1}[0-3]{1})$";
                    break;
                case "iyr":
                    regex = "^[2]{1}[0-9]{1}([1]{1}[0-9]{1}|[2]{1}[0]{1})$";
                    break;
                case "eyr":
                    regex = "^[2]{1}[0-9]{1}([2]{1}[0-9]{1}|[3]{1}[0]{1})$";
                    break;
                case "hgt":
                    regex = "^(1([5-8][0-9]|9[0-3])cm)|((59|6[0-9]|7[0-6])in)$";
                    break;
                case "hcl":
                    regex = "^#[0-9a-f]{6}$";
                    break;
                case "ecl":
                    regex = "^(amb|blu|brn|gry|grn|hzl|oth){1}$";
                    break;
                case "pid":
                    regex = "^[0-9]{9}$";
                    break;
                case "cid":
                    return true;
                default:
                    regex = "";
                    break;

            }
            return Regex.Match(split[1], regex).Success;
        }

    }

}
