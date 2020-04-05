# MVMP3

[![Actions Status](https://github.com/tetsuo13/MVMP3/workflows/Continuous%20integration/badge.svg)](https://github.com/tetsuo13/MVMP3/actions)

MVMP3 (alternatively named after "rename MP3") is a tool for processing MP3 files based on their ID3 tags. It was originally created to rebuild an export of the iPod library which contains cryptic file and directory names into a more symantic file structure.

Or, more to the point, to take an export of an iPod from many years ago that I no longer have and turn it into something more usable. It's true that ID3 tags are more meaningful than file names and leaving the export as it is will most likely be parsed correctly by any music player, however I prefer to have descriptive file names so I'm not beholden to music players.

The output is based on artist > album > song heiarchy.

As a further example of the iPod library, this is a slice of the export:

```
iPod_Control
+- Music
   +- F00
   | +- ADAZ.mp3
   | +- AQDK.mp3
   | ...
   +- F01
   | +- AFJW.mp3
   | +- AHXH.mp3
   | ...
   ...
```

and this would be the result of running MVMP3 against that export:

```
Artist 1
  Album
    Song #2
    Song #9
Artist 2
  Album
    Song #8
    Sone #14
```

## Usage

Start by specifying the base directory where to find the MP3s.

```
mvmp3 --source=C:\iPad_Control
```

MVMP3 will make a copy of all files into the same directory as the executable in a new subdirectory.

Supply the `--verbose` flag to see details during processing.
