Oi ya nobz! We'z got a new Dakka Tool from one uh da Weird Boyz!
======

The Pit Boss is a Windows program for Foxhole I am working on that will allow for more accurate relaying of artillery Az/Dist readouts from a spotter to the gunners.</br>

This is accomplished by providing both the spotter and the gunners with an overlay, similar to Discord or Steam, that will show the spotter's currently intended Az/Dist for the guns to fire at.</br>

Az/Dist adjustments can be made by the spotter using keyboard shortcuts, rather than having to manually type them into a text box every time.</br>

### To Install:
Get the pre-release version from my GitHub repository here: </br>
https://github.com/Witch-PhD/PitBoss/releases </br>
The program is set up so that there is only one file, PitBoss.exe, that can be double clicked to run.

The Pit Boss requires Microsoft .Net 8.0 Runtime, which you may be prompted to download if it is not already installed.
If for some reason the prompt does not work, it can be downloaded directly from Microsoft's website using this link:</br>
https://dotnet.microsoft.com/en-us/download/dotnet/8.0 </br>
Specifically, you will want to get the .Net Desktop Runtime. Most likely x64, but if your computer is older you may need x86 instead.

### To start using it:
The spotter will open The Pit Boss, and click on the "Open Connections" button on the Spotter tab.
Any gunners wishing to join will open their own copies of The Pit Boss and enter their Spotter's IP address and port number then click "Connect"</br>
Find your IP with this website:</br>
https://www.whatismyip.com/

Both the Gunners and Spotter can open an adjustable overlay window with the "Toggle Overlay" button on their respective tabs.

### Spotter shotcuts (as of v0.0.1):
```
Increase Az by 1 degree: Right Ctrl + Right Arrow
Increase Az by 15 degrees: Right Shift + Right Arrow
Decrease Az by 1 degree: Right Ctrl + Left Arrow
Decrease Az by 15 degrees: Right Shift + Left Arrow

Increase Dist by 1 meter: Right Ctrl + Up Arrow
Increase Dist by 10 meters: Right Shift + Up Arrow
Decrease Dist by 1 meter: Right Ctrl + Down Arrow
Decrease Dist by 10 meters: Right Shift + Down Arrow

Send the coords to all gunners: Right Ctrl + Number Pad Zero (0)
```
</br>
</br>

This is a very new project and will necessarily be quite bare bones and prone to errors and crashes for the moment, but I hope with your help in testing it we can make this tool into something that will be indispensible on ops in the future.

Any feedback, positive and (especially) negative is welcome and encouraged. Please let me know of any bugs you run into. Feel free to either leave your feedback in [Discussions](https://github.com/Witch-PhD/PitBoss/discussions), add an [Issue](https://github.com/Witch-PhD/PitBoss/issues) on the GitHub repository, or DM me directly on Discord.
