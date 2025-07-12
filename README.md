# Daily Tanakh Reading 929

<img width="697" height="530" alt="Screenshot 2025-07-12 at 03 43 31" src="https://github.com/user-attachments/assets/d9b2fe53-91bc-4fb6-8fe4-d27b8abc97f9" />

I created this app for terminal integration to show daily Tanakh readings.

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
