# remux features
- Console app that automates remuxing of single video files or whole folders to mkv using **mkvmerge**
- Optionally recursively remux folders
- Moves src files to recycle bin after remuxing

## Prerequisites
- Must have [mkvmerge](https://mkvtoolnix.download/downloads.html#windows) path in environment variables

## Usage
### Converting a single file to mkv
`remux c:\users\gene\videos\myvid.mp4`

### Converting all files in a folder to mkv
`remux "c:\users\gene\videos\my vacation"`

### Recursively convert all files in a folder to mkv
`remux "c:\users\gene\videos\my vacation" -r`

or

`remux "c:\users\gene\videos\my vacation" --recursive`

*Note: This will follow folder paths all the way to the end*

# Credits
- [mkvmerge](https://mkvtoolnix.download)
