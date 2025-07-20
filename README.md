# Daily Tanakh Reading 929

<img width="697" height="530" alt="Screenshot 2025-07-20 at 03 03 04" src="https://github.com/user-attachments/assets/f11e63c9-fcbf-492e-b8af-ebcc6e23d672" />

I created this app for terminal integration to show daily Tanakh readings.

## Usage

```shell
# This just shows the daily Chapter and summary if it exists
dailylearning

# This updates the database of chapter summaries from Data/summaries.json file (snapshot of OpenScripture API as of July 2025)
dailylearning update

# This updates the database of chapter summaries directly from OpenScripture API
dailylearning fetch
```

## Building

`dotnet publish -c Release -r osx-arm64 --self-contained`

This command will generate build files in `./bin/Release/net9.0/osx-arm64/publish`. There are files next to `publish` folder that are generated but I don't know what they do and I didn't find them necesarry for integration.

> Replace osx-arm64 with a different platform if you're not on macOS

## Folder structure

I like to keep my local apps and symbolic links in user root directory using this structure

```
user ┐
     ├── .homemadeapps/apps
     └── .homemadeapps/bin
```

Copy the whole `publish` folder to `.homemadeapps/apps` and rename it to something relevand, for example DailyLearning. Then create a symbolic link with:

```bash
ln -s ~/.homemadeapps/apps/DailyLearning/DailyLearning ~/.homemadeapps/bin/dailylearning
chmod +x ~/.homemadeapps/bin/dailylearning
```

## Terminal integration

In your `.zshrc` include

```bash
export PATH="$HOME/.homemadeapps/bin:$PATH"
dailylearning
```

> Replace .zshrc with a differenct file if you're not using zsh
