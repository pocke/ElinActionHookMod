# Elin Action Hook mod

Elinでゲームイベントにフックし、アクションを実行できるようにするModです。

例えば以下のような設定ができます。

* ネフィアに入る前に戦闘用装備に切り替える。
* ネフィアに入った後に魔法の地図を唱えて、探知機を起動する。
* クラフト前に、クラフト用の装備に切り替える。

## Configuration

このmodを使用するためには、`hooks.csv`ファイルをmodがインストールされたディレクトリに配置する必要があります。
このファイルの形式は以下の通りです。

```csv
EventType,SubType,Phase,ActionType,ActionArgs
EnterZone,PCFaction,Before,ChangeEquipment,6
```

CSVファイルの例は`sample/`ディレクトリ下にあります。このファイルをインストールディレクトリにコピー、編集してください。

このCSVファイルにはイベントとアクションのペアを定義します。以下に詳細を示します。

### イベント

イベントは「いつアクションを実行するか」を定義します。CSVの`EventType`、`SubType`、`Phase`の列が該当します。

* `EventType`: イベントの種類を指定します。
* `SubType`: イベントのサブタイプを指定します。
  * `EventType`によっては`SubType`で詳細な条件を指定する必要があります。
* `Phase`: イベントが起きる前(`Before`)か後(`After`)かを指定します。

`EventType`の一覧と、それぞれ指定できる`SubType`と`Phase`は以下の通りです。

| EventType       | SubType            | Phase             |
| --------------- | ------------------ | ----------------- |
| `EnterZone`     | ゾーンの種類を指定 | `Before`, `After` |
| `Sleep`         | 指定なし           | `Before`のみ      |
| `GoDownStairs`  | 指定なし           | `Before`のみ      |
| `GoUpStairs`    | 指定なし           | `Before`のみ      |
| `StartCrafting` | スキルを指定       | `Before`のみ      |  

それぞれのイベントの詳細は以下の通りです。

#### `EnterZone`

新しいゾーンに入るとき(マップが切り替わるとき)にトリガーされます。

`Phase`には以下を指定できます。

* `Before`
  * ゾーンに入る直前にトリガーされます。
  * プレイヤーがグローバルマップからゾーンに入るときのみトリガーされます。
* `After`
  * ゾーンに入った後にトリガーされます。
  * ネフィアで階段を使った後など、グローバルマップ以外のケースもトリガーされます。

`SubType`には以下のいずれかのゾーンの種類を指定します。

* `Nefia`
  * ネフィアに入るときにトリガーされます。
* `PCFaction`
  * プレイヤーが所属するファクションのゾーンに入るときにトリガーされます。

#### `Sleep`

プレイヤーがホットバーから寝るときにトリガーされます。

指定できる`Phase`は`Before`のみです。
`SubType`は指定できません。

#### `GoDownStairs`, `GoUpStairs`

それぞれプレイヤーが階段を下りるとき、上るときにトリガーされます。

`Phase`には`Before`のみを指定できます。`After`相当は`EnterZone`イベントを使用してください。
`SubType`は指定できません。

#### `StartCrafting`

プレイヤーがクラフトを開始するときにトリガーされます。
木工の机などのクラフト設備や、製材機などの加工設備が該当します。

`Phase`には`Before`のみを指定できます。
`SubType`にはクラフト設備が使用するスキルを指定します。指定できるスキルは以下の通りです。

* `Carpentry`: 木工
* `Blacksmith`: 鍛冶
* `Alchemy`:  錬金
* `Sculpture`: 彫刻
* `Jewelry`: 宝石細工
* `Weaving`: 裁縫
* `Handicraft`: 製作
* `Cooking`: 料理
* `Reading`: 読書


### アクション

アクションは「イベントが発生したときに何をするか」を定義します。CSVの`ActionType`、`ActionArgs`の列が該当します。

* `ActionType`: 実行するアクションの種類を指定します。
* `ActionArgs`: アクションに必要な引数を指定します。
  * 引数が不要なアクションは`ActionArgs`を空にします。

`ActionType`の一覧と、それぞれ指定できる`ActionArgs`は以下の通りです。

| ActionType | ActionArgs |
|------------|------------|
| `ShowMessage`      | 指定なし |
| `ChangeEquipment` | 数値 |
| `ChangeToolbelt` | 数値/数値 |
| `Save`     | 指定なし |

それぞれのアクションの詳細は以下の通りです。

#### `ShowMessage`

ポップアップでメッセージを表示します。主にデバッグ用途に使用します。

`ActionArgs`は指定しません。

#### `ChangeEquipment`

ホットバーウィジェットのプリセットを使用して装備セットを変更します。

`ActionArgs`にはホットバーのスロット番号を1から始まるインデックスで指定します。
例えば`F6`キーが対応するスロットを使用する場合、`6`を指定します。

#### `ChangeToolbelt`

ツールベルトからアイテムを選択します。

`ActionArgs`には2つの数値をスラッシュ(`/`)で区切って指定します。
1つ目の数値は、ツールベルトのページ番号(1または2)を指定します(controlキーで切り替えるものです)。
2つ目の数値は、ツールベルトのアイテムの位置(1-9)を指定します。

#### `Save`

ゲームを保存します。

`ActionArgs`は指定しません。

## Tips

* 同じイベントが複数の行で使われている場合、上から順にアクションが実行されます。
* CSVファイルにエラーがある場合、ゲーム上のポップアップと`Player.log`にエラーを表示します。
