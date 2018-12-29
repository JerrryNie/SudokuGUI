using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Get();
        }
        TextBox[,] Boxes = new TextBox[9, 9];
        int[,] Sudo = new int[9, 9];
        int[] Shift = new int[9] { 0, 3, 6, 1, 4, 7, 2, 5, 8};
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void GenerateNew()
        {
            Random random = new Random();
            for (int i = 0; i < 9; i++)
            {
                Sudo[0, i] = i + 1;
            }
            for (int i = 0; i < 10; i++)//生成第一行
            {

                int idx1 = random.Next() % 9;
                int idx2 = random.Next() % 9;
                int cur = Sudo[0, idx1];
                Sudo[0, idx1] = Sudo[0, idx2];
                Sudo[0, idx2] = cur;
            }
            for (int row = 1; row < 9; row++)
            {//生成其余8行
                for (int col = 0; col < 9; col++)
                {
                    Sudo[row, (col + Shift[row]) % 9] = Sudo[0, col];
                }
            }
            /*for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    int idx = random.Next(0, 9);
                    if (Sudo[row, idx] != 0)
                    {
                        CntNine[row / 3, col / 3]++;
                        cnt++;
                    }
                    Sudo[row, idx] = 0;
                }
            }*/
            for(int row = 0; row < 9; row += 3)//挖空
            {
                for (int col = 0; col < 9; col += 3)
                {
                    int cnt_NineBoxes = 0;
                    while(cnt_NineBoxes < 2)
                    {
                        for(int k = 0; k < 3; k++)
                        {
                            for(int m = 0; m < 3; m++)
                            {
                                int X = row + k;
                                int Y = col + m;
                                int Rand = random.Next(0, 9);
                                if(Rand >= 4)//超过50%的概率挖空
                                {
                                    if(Sudo[X, Y] != 0)
                                    {
                                        if (cnt_NineBoxes >= 6) break;
                                        cnt_NineBoxes++;
                                        Sudo[X, Y] = 0;//挖空
                                    }
                                }
                            }
                            if (cnt_NineBoxes >= 6) break;
                        }
                        if (cnt_NineBoxes >= 6) break;
                    }
                }
            }
            //addition();

        }
        public void Get()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Boxes[row, col] = (TextBox)tableLayoutPanel1.GetControlFromPosition(col, row);
                }
            }

        }
        public bool CheckValid()
        {
           // Get();//获得填空后的情况
            int[] BitMap = new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 9; i++)
            {//检查行方向合法性

                for (int j = 0; j < 9; j++)
                {
                    BitMap[Sudo[i, j] - 1] = 1;
                }
                for (int j = 0; j < 9; j++)
                {
                    if (BitMap[j] == 0)
                    {
                        String Str = "行非法" + i;
                        System.Windows.Forms.MessageBox.Show(Str);
                        //printf("行非法：%d, %d\n", i, j);
                        return false;//出现不合法数独
                        
                    }
                }
                for (int j = 0; j < 9; j++) BitMap[j] = 0;
            }

            for (int j = 0; j < 9; j++)
            {//检查列方向合法性

                for (int i = 0; i < 9; i++)
                {
                    BitMap[Sudo[i, j] - 1] = 1;
                }
                for (int i = 0; i < 9; i++)
                {
                    if (BitMap[i] == 0)
                    {
                        String Str = "列非法" + j;
                        System.Windows.Forms.MessageBox.Show(Str);
                        //printf("列非法：%d, %d\n", i, j);
                        return false;//出现不合法数独
                    }
                }
                for (int m = 0; m < 9; m++) BitMap[m] = 0;
            }
            int[] dr = new int[]{ 0, 1, 2 };
            int[] dc = new int[]{ 0, 1, 2 };
            /*printf("---------------\n");
            for (int i = 1; i <= 9; i++) {
                for (int j = 1; j <= 9; j++) {
                    printf("%d", SudoMat[i][j]);
                }
                puts("");
            }*/
            for (int i = 0; i < 9; i += 3)
            {

                for (int j = 0; j < 9; j += 3)
                {

                    for (int k = 0; k <= 2; k++)
                    {
                        for (int m = 0; m <= 2; m++)
                        {
                            BitMap[Sudo[(i + dr[k]), (j + dc[m])] - 1] = 1;
                        }
                    }
                    for (int m = 0; m < 9; m++)
                    {
                        if (BitMap[m] == 0)
                        {
                            String Str = "3*3非法: " + i + "," + j;
                            System.Windows.Forms.MessageBox.Show(Str);
                            //printf("3*3非法：%d, %d\n", i, j);
                            return false;//出现不合法数独
                        }
                    }
                    for (int m = 0; m < 9; m++) BitMap[m] = 0;
                }
            }
            return true;

        }
        public void Show()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Sudo[row, col] == 0)
                    {
                        TextBox cur = Boxes[row, col];
                        cur.ReadOnly = false;
                        cur.ForeColor = Color.Black;
                    }
                    else
                    {
                        TextBox cur = Boxes[row, col];
                        Color curcolor = cur.BackColor;
                        cur.Text = Sudo[row, col].ToString();
                        cur.ReadOnly = true;
                        cur.BackColor = curcolor;
                        cur.ForeColor = Color.Red;
                    }
                }
            }

        }
        public void GetUserAns()//得到用户填写的答案
        {
            for(int row = 0; row < 9; row++)
            {
                for(int col = 0; col < 9; col++)
                {
                    if(Sudo[row, col] == 0) { //给Sudo以答案
                        String Str = Boxes[row, col].Text;
                        bool b = int.TryParse(Str, out Sudo[row, col]);
                        
                        //bool parsed = Int32.TryParse(Str, out Sudo[row, col]);
                    }
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            String s36= sender.ToString();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            String s11 = sender.ToString();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            String s12 = sender.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            String s13 = sender.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            String s14 = sender.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            String s15 = sender.ToString();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            String s16 = sender.ToString();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            String s17 = sender.ToString();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            String s18 = sender.ToString();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            String s19 = sender.ToString();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            String s21 = sender.ToString();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            String s22 = sender.ToString();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            String s23 = sender.ToString();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            String s24 = sender.ToString();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            String s25 = sender.ToString();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            String s26 = sender.ToString();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            String s27 = sender.ToString();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            String s28 = sender.ToString();
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            String s29 = sender.ToString();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            String s31= sender.ToString();
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            String s32 = sender.ToString();
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            String s33 = sender.ToString();
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            String s34 = sender.ToString();
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            String s35 = sender.ToString();
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            String s37 = sender.ToString();
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            String s38 = sender.ToString();
        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            String s39 = sender.ToString();
        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            String s41 = sender.ToString();
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            String s42 = sender.ToString();
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            String s43 = sender.ToString();
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            String s44 = sender.ToString();
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            String s45 = sender.ToString();
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            String s46 = sender.ToString();
        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {
            String s47 = sender.ToString();
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            String s48 = sender.ToString();
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            String s49 = sender.ToString();
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            String s51 = sender.ToString();
        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {
            String s52 = sender.ToString();
        }

        private void textBox39_TextChanged(object sender, EventArgs e)
        {
            String s53 = sender.ToString();
        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {
            String s54 = sender.ToString();
        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {
            String s55 = sender.ToString();
        }

        private void textBox42_TextChanged(object sender, EventArgs e)
        {
            String s56 = sender.ToString();
        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            String s57 = sender.ToString();
        }

        private void textBox44_TextChanged(object sender, EventArgs e)
        {
            String s58 = sender.ToString();
        }

        private void textBox45_TextChanged(object sender, EventArgs e)
        {
            String s59 = sender.ToString();
        }

        private void textBox46_TextChanged(object sender, EventArgs e)
        {
            String s61 = sender.ToString();
        }

        private void textBox47_TextChanged(object sender, EventArgs e)
        {
            String s62 = sender.ToString();
        }

        private void textBox48_TextChanged(object sender, EventArgs e)
        {
            String s63 = sender.ToString();
        }

        private void textBox49_TextChanged(object sender, EventArgs e)
        {
            String s64 = sender.ToString();
        }

        private void textBox50_TextChanged(object sender, EventArgs e)
        {
            String s65 = sender.ToString();
        }

        private void textBox51_TextChanged(object sender, EventArgs e)
        {
            String s66 = sender.ToString();
        }

        private void textBox52_TextChanged(object sender, EventArgs e)
        {
            String s67= sender.ToString();
        }

        private void textBox53_TextChanged(object sender, EventArgs e)
        {
            String s68 = sender.ToString();
        }

        private void textBox54_TextChanged(object sender, EventArgs e)
        {
            String s69 = sender.ToString();
        }

        private void textBox55_TextChanged(object sender, EventArgs e)
        {
            String s71 = sender.ToString();
        }

        private void textBox56_TextChanged(object sender, EventArgs e)
        {
            String s72 = sender.ToString();
        }

        private void textBox57_TextChanged(object sender, EventArgs e)
        {
            String s73 = sender.ToString();
        }

        private void textBox58_TextChanged(object sender, EventArgs e)
        {
            String s74 = sender.ToString();
        }

        private void textBox59_TextChanged(object sender, EventArgs e)
        {
            String s75 = sender.ToString();
        }

        private void textBox60_TextChanged(object sender, EventArgs e)
        {
            String s76 = sender.ToString();
        }

        private void textBox61_TextChanged(object sender, EventArgs e)
        {
            String s77 = sender.ToString();
        }

        private void textBox62_TextChanged(object sender, EventArgs e)
        {
            String s78 = sender.ToString();
        }

        private void textBox63_TextChanged(object sender, EventArgs e)
        {
            String s79 = sender.ToString();
        }

        private void textBox64_TextChanged(object sender, EventArgs e)
        {
            String s81 = sender.ToString();
        }

        private void textBox65_TextChanged(object sender, EventArgs e)
        {
            String s82 = sender.ToString();
        }

        private void textBox66_TextChanged(object sender, EventArgs e)
        {
            String s83 = sender.ToString();
        }

        private void textBox67_TextChanged(object sender, EventArgs e)
        {
            String s84 = sender.ToString();
        }

        private void textBox68_TextChanged(object sender, EventArgs e)
        {
            String s85 = sender.ToString();
        }

        private void textBox69_TextChanged(object sender, EventArgs e)
        {
            String s86 = sender.ToString();
        }

        private void textBox70_TextChanged(object sender, EventArgs e)
        {
            String s87 = sender.ToString();
        }

        private void textBox71_TextChanged(object sender, EventArgs e)
        {
            String s88 = sender.ToString();
        }

        private void textBox72_TextChanged(object sender, EventArgs e)
        {
            String s89 = sender.ToString();
        }

        private void textBox73_TextChanged(object sender, EventArgs e)
        {
            String s91 = sender.ToString();
        }

        private void textBox74_TextChanged(object sender, EventArgs e)
        {
            String s92 = sender.ToString();
        }

        private void textBox75_TextChanged(object sender, EventArgs e)
        {
            String s93 = sender.ToString();
        }

        private void textBox76_TextChanged(object sender, EventArgs e)
        {
            String s94 = sender.ToString();
        }

        private void textBox77_TextChanged(object sender, EventArgs e)
        {
            String s95 = sender.ToString();
        }

        private void textBox78_TextChanged(object sender, EventArgs e)
        {
            String s96 = sender.ToString();
        }

        private void textBox79_TextChanged(object sender, EventArgs e)
        {
            String s97 = sender.ToString();
        }

        private void textBox80_TextChanged(object sender, EventArgs e)
        {
            String s98 = sender.ToString();
        }

        private void textBox81_TextChanged(object sender, EventArgs e)
        {
            String s99 = sender.ToString();
        }
        int flag = 0;
        private void button1_Click(object sender, EventArgs e)//生成新数独
        {
            for(int row = 0; row < 9; row++)
            {
                for(int col = 0; col < 9; col++)
                {
                    Sudo[row, col] = 0;
                    Boxes[row, col].Text = "";
                }
            }
            GenerateNew();

            Show();
        }

        public bool  GetSudokuStatus()
        {
            for(int row = 0; row < 9; row++)
            {
                for(int col = 0; col < 9; col++)
                {
                    if(Sudo[row, col] == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("数独尚未完成！");
                        return false;
                    }
                    else if(!(Sudo[row, col] > 0 && Sudo[row, col] <= 9))
                    {
                        String Str = Sudo[row, col].ToString();
                        System.Windows.Forms.MessageBox.Show(Str);
                        return false;
                    }
                }
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)//提交答案
        {

            GetUserAns();
            bool Status = GetSudokuStatus();
            if (Status && CheckValid())//填写正确
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        Sudo[row, col] = 0;
                        Boxes[row, col].Text = "";
                    }
                }
                System.Windows.Forms.MessageBox.Show("答案正确！");
                GenerateNew();
                Show();
            }
            else if(Status)//填写错误
            {
                System.Windows.Forms.MessageBox.Show("答案错误！");
            }
        }
    }
}
