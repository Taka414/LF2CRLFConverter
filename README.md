# LF2CRLFConverter

Tool to convert files from LF to CRLF safely in C#

## Usage

Format:

(1) �`�F�b�N���[�h���s:
my.exe /check $dir
  ${dir}�Ŏw�肵���t�H���_���ċA�I�Ɍ������ăG���R�[�h�Ɗg���q���W�v���ĉ�ʂɏo�͂���

(2) �ϊ����s:
my.exe $dir
my.exe /convert $dir [${extensions-file-path}]
  ${dir}�Ŏw�肵���t�H���_���ċA�I�Ɍ�������
  ${extensions-file-path}�ɋL�ڂ��ꂽ�g���q�̃��X�g�Ɉ�v����t�@�C���̉��s�R�[�h��LF �� CRLF�ɕϊ�����

(3) �w���v�\��
my.exe /?
  �g�����̐�����\������i"note.txt" �̓��e���R���\�[���ɕ\�������j

## extensions.txt

���̃t�@�C���Ƀt�@�C���ɕϊ��������t�@�C���̊g���q��񋓂���

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
