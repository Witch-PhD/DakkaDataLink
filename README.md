Oi ya nobz! We'z got a new Dakka Tool from one uh da Weird Boyz!
======

![82DK](https://github.com/user-attachments/assets/e51c404e-2b40-4184-85b7-e6c2132cebc1)

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

Your Foxhole video settings must be set to Windowed Fullscreen or Windowed.</br>
Regular fullscreen may not work correctly.

Due to the peer-to-peer nature of the program, the person that is the server will need to set up Port Forwarding in their router for port number 50082.</br>
If you do not know how to do that, this is a good starting point:</br>
[How To Set Up Port Forwarding](https://support.source-elements.com/source-elements/step-by-step-port-forwarding-guide)

The server (could be anyone, spotter or gunner) will open The Pit Boss, and click on the "Start as Server" button on the Options tab.
Any clients wishing to join will open their own copies of The Pit Boss and enter the IP address of the person who is the server then click "Connect To Server"</br>
Find your IP with this website:</br>
https://www.whatismyip.com/

Both the Gunners and Spotter can open an adjustable overlay window with the "Toggle Overlay" button.

Spotters can customize their keyboard shortcuts to increase and descrease azimuth and distance by using the Spotter Options panel in the Options tab.

### Artillery Profiles
When spotting, it is useful to change the distance in a set number of meters for that specific weapon system, known as "ticks."</br>
To set the tick value, select the artillery profile in the Options tab that is appropriate for the weapon system in use.</br>
PitBoss will take care of the rest.

</br>
</br>

This is a very new project and will necessarily be quite bare bones and prone to errors and crashes for the moment, but I hope with your help in testing it we can make this tool into something that will be indispensible on ops in the future.

Any feedback, positive and (especially) negative is welcome and encouraged. Please let me know of any bugs you run into. Feel free to either leave your feedback in [Discussions](https://github.com/Witch-PhD/PitBoss/discussions), add an [Issue](https://github.com/Witch-PhD/PitBoss/issues) on the GitHub repository, or DM me directly on Discord.

Special thanks to all the people who have helped me test this so far, in particular:</br>
[82DK-A] Einherj</br>
[82DK-A] Huners</br>
Avail</br>
Kyuss/Gustavo</br>
