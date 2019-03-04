# TODO

1. 能分則不能合

    ```txt
    后王	后王
    后灯	後燈
    ```

2. 先分詞、再轉字

    1. 分詞 (將句子分割): `STPhrases.txt` ( 48,933 Phrases )
    2. 換字 (將單字換對): `STPhrases.txt`、`STCharacters.txt` ( 3,898 Characters )
    3. 調字 (將單字調整): `TWVariants.txt` ( 36 Characters )
    4. 換詞 (將句子調整): `TWPhrasesIT.txt`
    5. 換詞 (將句子調整): `TWPhrasesOther.txt`
    6. 換詞 (將句子調整): `TWPhrases-Angular.txt`

3. 改用 `jq` 來修改 `navigation.json` 檔案

    [Modify a key-value in a json using jq](https://stackoverflow.com/questions/42716734/modify-a-key-value-in-a-json-using-jq)

    ```sh
    tmp=$(mktemp)
    jq '.TopBar = "abcde"' navigation.json > "$tmp" && mv "$tmp" test.json
    ```

    ```sh
    tmp=$(mktemp)
    jq 'del(.TopBar[5])' navigation.json > "$tmp" && mv "$tmp" navigation.json
    ```
