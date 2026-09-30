using Avalonia.Controls;
﻿using Avalonia.Threading;
using ShareX.AvaloniaUI.Integration;
using ShareX.AvaloniaUI.Theming;
using System;
using System.Threading.Tasks;

namespace ShareX.Linux;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args){
        AvaloniaBootstrapper.Initialize(
            args,
            startup: async () =>
            {
                ThemeManager.Configure(new ApplicationThemeOptions());


                var capBtn = new Button{
                    Content = "Press 2 captureee",
                    FontSize = 18,
                    Margin = new Avalonia.Thickness(30),
                    Padding = new Avalonia.Thickness(20,10)
                };

                    capBtn.Click+=async (_,_) => {
                        var proc = new System.Diagnostics.Process{
                            StartInfo = new System.Diagnostics.ProcessStartInfo{
                                FileName = "gdbus",
                                Arguments =
                                        "call --session " +
                                        "--dest org.freedesktop.portal.Desktop " +
                                        "--object-path /org/freedesktop/portal/desktop " +
                                        "--method org.freedesktop.portal.Screenshot.Screenshot " +
                                        "\"\" \"{'interactive': <true>}\"",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };
                        proc.Start();
                        string output = await proc.StandardOutput.ReadToEndAsync();
                        await proc.WaitForExitAsync();
                        Console.WriteLine(output);
                };

                var window = new Window{
                    Title = "ShareX GAY PORT",
                    Width = 1000,
                    Height = 650,
                    Content = capBtn,
                };

                window.Show();
                await Task.CompletedTask;
            },
            shutdown: () => { }

        );
        AvaloniaBootstrapper.Run();
    }
}
