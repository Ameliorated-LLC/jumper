﻿using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace JumpServer;

public static class Utilities
{
    
    public static (string Output, string Error, int ExitCode) ExecuteCommand(string command, string[] arguments, string? input = null)
    {
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using (Process process = new Process())
        {
            process.StartInfo.FileName = command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            
            foreach (string argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }
            
            using var outputCompleted = new SemaphoreSlim(0, 1);
            using var errorCompleted = new SemaphoreSlim(0, 1);

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    outputBuilder.AppendLine(args.Data);
                else
                    // ReSharper disable once AccessToDisposedClosure
                    outputCompleted.Release();
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)

                    errorBuilder.AppendLine(args.Data);
                else
                    // ReSharper disable once AccessToDisposedClosure
                    errorCompleted.Release();
            };

            try
            {
                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (input != null)
                {
                    using (var stdin = process.StandardInput)
                    {
                        stdin.WriteLine(input);
                        stdin.Flush();
                    }
                }
                
                process.WaitForExit();
                
                outputCompleted.Wait();
                errorCompleted.Wait();

                int exitCode = process.ExitCode;

                return (outputBuilder.ToString(), errorBuilder.ToString(), exitCode);
            }
            catch (Exception ex)
            {
                return ($"Exception: {ex.Message}", ex.Message, -1);
            }
        }
    }
}

public enum AnsiColor
{
    // Standard colors
    Black = 0,
    Maroon = 1,
    Green = 2,
    Olive = 3,
    Navy = 4,
    Purple = 5,
    Teal = 6,
    Silver = 7,
    Grey = 8,
    Red = 9,
    Lime = 10,
    Yellow = 11,
    Blue = 12,
    Fuchsia = 13,
    Aqua = 14,
    White = 15,

    // High-intensity colors
    Grey0 = 16,
    NavyBlue = 17,
    DarkBlue = 18,
    Blue3 = 19,
    Blue3_1 = 20,
    Blue1 = 21,
    DarkGreen = 22,
    DeepSkyBlue4 = 23,
    DeepSkyBlue4_1 = 24,
    DeepSkyBlue4_2 = 25,
    DodgerBlue3 = 26,
    DodgerBlue2 = 27,
    Green4 = 28,
    SpringGreen4 = 29,
    Turquoise4 = 30,
    DeepSkyBlue3 = 31,
    DeepSkyBlue3_1 = 32,
    DodgerBlue1 = 33,
    Green3 = 34,
    SpringGreen3 = 35,
    DarkCyan = 36,
    LightSeaGreen = 37,
    DeepSkyBlue2 = 38,
    DeepSkyBlue1 = 39,
    Green3_1 = 40,
    SpringGreen3_1 = 41,
    SpringGreen2 = 42,
    Cyan3 = 43,
    DarkTurquoise = 44,
    Turquoise2 = 45,
    Green1 = 46,
    SpringGreen2_1 = 47,
    SpringGreen1 = 48,
    MediumSpringGreen = 49,
    Cyan2 = 50,
    Cyan1 = 51,
    DarkRed = 52,
    DeepPink4 = 53,
    Purple4 = 54,
    Purple4_1 = 55,
    Purple3 = 56,
    BlueViolet = 57,
    Orange4 = 58,
    Grey37 = 59,
    MediumPurple4 = 60,
    SlateBlue3 = 61,
    SlateBlue3_1 = 62,
    RoyalBlue1 = 63,
    Chartreuse4 = 64,
    DarkSeaGreen4 = 65,
    PaleTurquoise4 = 66,
    SteelBlue = 67,
    SteelBlue3 = 68,
    CornflowerBlue = 69,
    Chartreuse3 = 70,
    DarkSeaGreen4_1 = 71,
    CadetBlue = 72,
    CadetBlue_1 = 73,
    SkyBlue3 = 74,
    SteelBlue1 = 75,
    Chartreuse3_1 = 76,
    PaleGreen3 = 77,
    SeaGreen3 = 78,
    Aquamarine3 = 79,
    MediumTurquoise = 80,
    SteelBlue1_1 = 81,
    Chartreuse2 = 82,
    SeaGreen2 = 83,
    SeaGreen1 = 84,
    SeaGreen1_1 = 85,
    Aquamarine1 = 86,
    DarkSlateGray2 = 87,
    DarkRed_1 = 88,
    DeepPink4_1 = 89,
    DarkMagenta = 90,
    DarkMagenta_1 = 91,
    DarkViolet = 92,
    Purple_1 = 93,
    Orange4_1 = 94,
    LightPink4 = 95,
    Plum4 = 96,
    MediumPurple3 = 97,
    MediumPurple3_1 = 98,
    SlateBlue1 = 99,
    Yellow4 = 100,
    Wheat4 = 101,
    Grey53 = 102,
    LightSlateGrey = 103,
    MediumPurple = 104,
    LightSlateBlue = 105,
    Yellow4_1 = 106,
    DarkOliveGreen3 = 107,
    DarkSeaGreen = 108,
    LightSkyBlue3 = 109,
    LightSkyBlue3_1 = 110,
    SkyBlue2 = 111,
    Chartreuse2_1 = 112,
    DarkOliveGreen3_1 = 113,
    PaleGreen3_1 = 114,
    DarkSeaGreen3 = 115,
    DarkSlateGray3 = 116,
    SkyBlue1 = 117,
    Chartreuse1 = 118,
    LightGreen = 119,
    LightGreen_1 = 120,
    PaleGreen1 = 121,
    Aquamarine1_1 = 122,
    DarkSlateGray1 = 123,
    Red3 = 124,
    DeepPink4_2 = 125,
    MediumVioletRed = 126,
    Magenta3 = 127,
    DarkViolet_1 = 128,
    Purple1 = 129,
    DarkOrange3 = 130,
    IndianRed = 131,
    HotPink3 = 132,
    MediumOrchid3 = 133,
    MediumOrchid = 134,
    MediumPurple2 = 135,
    DarkGoldenrod = 136,
    LightSalmon3 = 137,
    RosyBrown = 138,
    Grey63 = 139,
    MediumPurple2_1 = 140,
    MediumPurple1 = 141,
    Gold3 = 142,
    DarkKhaki = 143,
    NavajoWhite3 = 144,
    Grey69 = 145,
    LightSteelBlue3 = 146,
    LightSteelBlue = 147,
    Yellow3 = 148,
    DarkOliveGreen3_2 = 149,
    DarkSeaGreen3_1 = 150,
    DarkSeaGreen2 = 151,
    LightCyan3 = 152,
    LightSkyBlue1 = 153,
    GreenYellow = 154,
    DarkOliveGreen2 = 155,
    PaleGreen1_1 = 156,
    DarkSeaGreen2_1 = 157,
    DarkSeaGreen1 = 158,
    PaleTurquoise1 = 159,
    Red3_1 = 160,
    DeepPink3 = 161,
    DeepPink3_1 = 162,
    Magenta3_1 = 163,
    Magenta3_2 = 164,
    Magenta2 = 165,
    DarkOrange3_1 = 166,
    IndianRed_1 = 167,
    HotPink3_1 = 168,
    HotPink2 = 169,
    Orchid = 170,
    MediumOrchid1 = 171,
    Orange3 = 172,
    LightSalmon3_1 = 173,
    LightPink3 = 174,
    Pink3 = 175,
    Plum3 = 176,
    Violet = 177,
    Gold3_1 = 178,
    LightGoldenrod3 = 179,
    Tan = 180,
    MistyRose3 = 181,
    Thistle3 = 182,
    Plum2 = 183,
    Yellow3_1 = 184,
    Khaki3 = 185,
    LightGoldenrod2 = 186,
    LightYellow3 = 187,
    Grey84 = 188,
    LightSteelBlue1 = 189,
    Yellow2 = 190,
    DarkOliveGreen1 = 191,
    DarkOliveGreen1_1 = 192,
    DarkSeaGreen1_1 = 193,
    Honeydew2 = 194,
    LightCyan1 = 195,
    Red1 = 196,
    DeepPink2 = 197,
    DeepPink1 = 198,
    DeepPink1_1 = 199,
    Magenta2_1 = 200,
    Magenta1 = 201,
    OrangeRed1 = 202,
    IndianRed1 = 203,
    IndianRed1_1 = 204,
    HotPink = 205,
    HotPink_1 = 206,
    MediumOrchid1_1 = 207,
    DarkOrange = 208,
    Salmon1 = 209,
    LightCoral = 210,
    PaleVioletRed1 = 211,
    Orchid2 = 212,
    Orchid1 = 213,
    Orange1 = 214,
    SandyBrown = 215,
    LightSalmon1 = 216,
    LightPink1 = 217,
    Pink1 = 218,
    Plum1 = 219,
    Gold1 = 220,
    LightGoldenrod2_1 = 221,
    LightGoldenrod2_2 = 222,
    NavajoWhite1 = 223,
    MistyRose1 = 224,
    Thistle1 = 225,
    Yellow1 = 226,
    LightGoldenrod1 = 227,
    Khaki1 = 228,
    Wheat1 = 229,
    Cornsilk1 = 230,
    Grey100 = 231,
    Grey3 = 232,
    Grey7 = 233,
    Grey11 = 234,
    Grey15 = 235,
    Grey19 = 236,
    Grey23 = 237,
    Grey27 = 238,
    Grey30 = 239,
    Grey35 = 240,
    Grey39 = 241,
    Grey42 = 242,
    Grey46 = 243,
    Grey50 = 244,
    Grey54 = 245,
    Grey58 = 246,
    Grey62 = 247,
    Grey66 = 248,
    Grey70 = 249,
    Grey74 = 250,
    Grey78 = 251,
    Grey82 = 252,
    Grey85 = 253,
    Grey89 = 254,
    Grey93 = 255
}