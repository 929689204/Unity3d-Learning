# Homework2-编程实践

## 题目要求

* 阅读以下游戏脚本

> Priests and Devils
Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck ！

### 简答题：

* play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )
* 列出游戏中提及的事物（Objects） 
> 岸(Bank)， 河流(River) ，牧师 (human)，魔鬼 (evil)， 船(boat)    
* 用表格列出玩家动作表（规则表）

| 行为 | 行为条件 |
| --- | --- |
| 牧师／恶魔上船 | 船上有空位且船在该岸 |
| 开船 | 船上有人 |
| 牧师/恶魔下船 | 船有人且靠在岸边 |
| 游戏胜利 | 6个人都到了对岸 |
| 游戏失败 | 有一边岸上牧师少于恶魔 |
