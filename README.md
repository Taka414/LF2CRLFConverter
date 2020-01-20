# LF2CRLFConverter

Tool to convert files from LF to CRLF safely in C#

## Usage

Format:

(1) チェックモード実行:
my.exe /check $dir
  ${dir}で指定したフォルダを再帰的に検索してエンコードと拡張子を集計して画面に出力する

(2) 変換実行:
my.exe $dir
my.exe /convert $dir [${extensions-file-path}]
  ${dir}で指定したフォルダを再帰的に検索して
  ${extensions-file-path}に記載された拡張子のリストに一致するファイルの改行コードをLF → CRLFに変換する

(3) ヘルプ表示
my.exe /?
  使い方の説明を表示する（"note.txt" の内容がコンソールに表示される）

## extensions.txt

このファイルにファイルに変換したいファイルの拡張子を列挙する

```txt
// extensions.txt

.c
.cpp
.h
.hpp
.txt
.gitignore
.log
.meta
.prefab
.cs
.XML
.asset
.anim
.controller
.unity
.json
.asmdef
.spriteatlas
```

### 確認環境

* Windows10
* .NET 4.7.2(.NET Coreでも動く)
* Visual Studio2019
