#!/bin/sh

cd willh

OPENCC=/usr/share/opencc

echo '建立 STPhrases-Angular.ocd 檔案'
##########################################

TMPFILE=`mktemp`
echo "Creating TEMP file: $TMPFILE"
cat ../data/dictionary/STPhrases.txt    >> $TMPFILE
cat ./STPhrases-Angular.txt            >> $TMPFILE
sudo opencc_dict -f text -t ocd -i $TMPFILE -o $OPENCC/STPhrases-Angular.ocd
sudo chmod 644 $OPENCC/STPhrases-Angular.ocd
rm -f $TMPFILE


echo '建立 STPhrases-Python.ocd 檔案'
##########################################

TMPFILE=`mktemp`
echo "Creating TEMP file: $TMPFILE"
cat ../data/dictionary/STPhrases.txt    >> $TMPFILE
cat ./STPhrases-Python.txt            >> $TMPFILE
sudo opencc_dict -f text -t ocd -i $TMPFILE -o $OPENCC/STPhrases-Python.ocd
sudo chmod 644 $OPENCC/STPhrases-Python.ocd
rm -f $TMPFILE


echo '建立 TWPhrases-Angular.ocd 檔案'
##########################################

TMPFILE=`mktemp`
echo "Creating TEMP file: $TMPFILE"
cat ../data/dictionary/TWPhrasesIT.txt    >> $TMPFILE
cat ../data/dictionary/TWPhrasesOther.txt >> $TMPFILE
cat ./TWPhrases-Other.txt                >> $TMPFILE
cat ./TWPhrases-Angular.txt              >> $TMPFILE
sudo opencc_dict -f text -t ocd -i $TMPFILE -o $OPENCC/TWPhrases-Angular.ocd
sudo chmod 644 $OPENCC/TWPhrases-Angular.ocd
rm -f $TMPFILE

echo '建立 TWPhrases-Python.ocd 檔案'
##########################################

TMPFILE=`mktemp`
echo "Creating TEMP file: $TMPFILE"
cat ../data/dictionary/TWPhrasesIT.txt    >> $TMPFILE
cat ../data/dictionary/TWPhrasesOther.txt >> $TMPFILE
cat ./TWPhrases-Other.txt                >> $TMPFILE
cat ./TWPhrases-Python.txt               >> $TMPFILE
sudo opencc_dict -f text -t ocd -i $TMPFILE -o $OPENCC/TWPhrases-Python.ocd
sudo chmod 644 $OPENCC/TWPhrases-Python.ocd
rm -f $TMPFILE


echo "複製 s2twp-Angular.json 到 $OPENCC"
##########################################

sudo cp s2twp-Angular.json $OPENCC/angular.json
sudo chmod 644 $OPENCC/angular.json


echo "複製 s2twp-Python.json 到 $OPENCC"
##########################################

sudo cp s2twp-Python.json $OPENCC/python.json
sudo chmod 644 $OPENCC/python.json


echo "測試結果"
##########################################

# opencc -c angular -i /mnt/g/a.txt -o /mnt/g/b.txt
# cat generated/docs/features.json | opencc -c angular | grep 自動完成

echo '開始建構你的 Angular 應用' | opencc -c angular
echo '開始建構你的 Python 應用' | opencc -c python

# 智能代码补全

# cd guide
# find . -name 'rx*.html' -type f -exec echo '{}' \; -exec opencc -c s2twp-Angular -i '{}' -o '{}' \;

# cd ../generated/docs/guide
# find . -name 'rx*.json' -type f -exec echo '{}' \; -exec opencc -c s2twp-Angular -i '{}' -o '{}' \;

# lite-server
