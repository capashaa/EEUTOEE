using EELVL;
using EEUniverse.Library;
using Flurl.Util;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace EEU2EEO
{
    public partial class Form1 : Form
    {
        private string blocks = $"{Directory.GetCurrentDirectory()}\\blocks.json";
        private Dictionary<int, int> list = new Dictionary<int, int>();
        public struct Block
        {
            public int Layer { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Id { get; set; }
            public object[] Extra { get; set; }

            public Block(int layer, int x, int y, int id, params object[] extra)
            {
                Layer = layer;
                X = x;
                Y = y;
                Id = id;
                Extra = extra;
            }

            public override string ToString() => $"({X}, {Y}, {Id}{(Extra?.Length > 0 ? $", {string.Join(", ", Extra)}" : "")})";
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbToken.Text))
            {
                if (!string.IsNullOrEmpty(txtbRoom.Text))
                {
                    if (File.Exists(blocks))
                    {
                        JObject o1 = JObject.Parse(File.ReadAllText(blocks));
                        foreach (var o in o1)
                        {
                            if (!list.ContainsKey(Convert.ToInt32(o.Key))) list.Add(Convert.ToInt32(o.Key), Convert.ToInt32(o.Value));
                        }
                    }
                    readData(txtbToken.Text, txtbRoom.Text);
                }
            }
        }
        private async void readData(string token, string roomid)
        {
            try
            {

                var client = new EEUniverse.Library.Client(txtbToken.Text);
                client.MaxBuffer = 1024 * 250;
                await client.ConnectAsync();
                IConnection connection = client.CreateWorldConnection(roomid);

                int[,,] totalBlocks = new int[2, 1, 1];
                connection.OnMessage += (s, m) =>
                {


                    if (m.Type == MessageType.Init)
                    {
                        var index = 11;
                        int count = 0;
                        int total = m.GetInt(10) * m.GetInt(9);
                        var blocks = new Block[2, m.GetInt(9) + 1, m.GetInt(10) + 1];
                        Level savelvl = new Level(m.GetInt(9), m.GetInt(10), 0);
                        savelvl.WorldName = m.GetString(6);
                        savelvl.OwnerName = m.GetString(7).ToLower();
                        savelvl.OwnerID = "made offline";
                        if (m.GetInt(8) != -1)
                        {
                            savelvl.BackgroundColor = (uint)(m.GetInt(8) & 0x00FFFFFF);
                        }
                        for (int y = 0; y < m.GetInt(10); y++)
                        {
                            for (int x = 0; x < m.GetInt(9); x++)
                            {
                                count++;
                                try
                                {
                                    int value = 0;
                                    if (m[index] is bool boolean)
                                    {
                                        blocks[1, x, y].Id = 0;
                                    }
                                    if (m[index++] is int iValue)
                                    {
                                        value = iValue;
                                        int background = value >> 16;
                                        int foreground = 65535 & value;

                                        blocks[1, x, y].Id = foreground;
                                        blocks[0, x, y].Id = background;

                                        //Sign
                                        if (blocks[1, x, y].Id >= 55 && blocks[1, x, y].Id <= 58)
                                        {
                                            if (blocks[1, x, y].Id == 55 || blocks[1, x, y].Id == 57)
                                            {
                                                savelvl[0, x, y] = new Blocks.SignBlock(385, (string)m[index++], 0);
                                            }
                                            else if (blocks[1, x, y].Id == 58)
                                            {
                                                savelvl[0, x, y] = new Blocks.SignBlock(385, (string)m[index++], 1);
                                            }
                                            else if (blocks[1, x, y].Id == 56)
                                            {
                                                savelvl[0, x, y] = new Blocks.SignBlock(385, (string)m[index++], 2);
                                            }
                                            index++;
                                        }
                                        //portals
                                        if (blocks[1, x, y].Id == 59)
                                        {
                                            savelvl[0, x, y] = new Blocks.PortalBlock(242, Convert.ToInt32(m[index++]), Convert.ToInt32(m[index++]), Convert.ToInt32(m[index++]));
                                            index++;

                                        }
                                        //Effects
                                        if (blocks[1, x, y].Id == 93 || blocks[1, x, y].Id == 94)
                                        {
                                            int val = Convert.ToInt32(m[index++]);
                                            if (blocks[1, x, y].Id == 93) savelvl[0, x, y] = new Blocks.NumberBlock(461, val == -1 ? 1000 : val);
                                            else if (blocks[1, x, y].Id == 94) savelvl[0, x, y] = new Blocks.EnumerableBlock(417, val);
                                        }
                                        //
                                        if (blocks[1, x, y].Id == 98 || blocks[1, x, y].Id == 101)
                                        {
                                            int ids = Convert.ToInt32(m[index++]);
                                            savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 101 ? 467 : 113, ids);
                                        }
                                        if (blocks[1, x, y].Id == 102 || blocks[1, x, y].Id == 99)
                                        {
                                            int ids = Convert.ToInt32(m[index++]);
                                            savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 102 ? 1620 : 1619, ids);
                                        }
                                        if (blocks[1, x, y].Id == 103 || blocks[1, x, y].Id == 100)
                                        {
                                            int ids = Convert.ToInt32(m[index++]);
                                            bool inverted = Convert.ToBoolean(m[index++]);
                                            if (!inverted)
                                            {
                                                savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 103 ? 1079 : 184, ids);
                                            }
                                            else
                                            {
                                                savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 103 ? 1080 : 185, ids);
                                            }
                                        }
                                        if (blocks[1, x, y].Id == 106 || blocks[1, x, y].Id == 120)
                                        {
                                            int rotation = Convert.ToInt32(m[index++]);
                                            switch (rotation)
                                            {
                                                case 0:
                                                    rotation = 1;
                                                    break;
                                                case 1:
                                                    rotation = 2;
                                                    break;
                                                case 2:
                                                    rotation = 3;
                                                    break;
                                                case 3:
                                                    rotation = 0;
                                                    break;
                                            }
                                            savelvl[0, x, y] = new Blocks.RotatableBlock(1052, rotation);
                                        }
                                        if (blocks[1, x, y].Id == 104 || blocks[1, x, y].Id == 105)
                                        {
                                            int coins = Convert.ToInt32(m[index++]);
                                            bool inverted = Convert.ToBoolean(m[index++]);
                                            if (!inverted)
                                            {
                                                savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 104 ? 43 : 213, coins);
                                            }
                                            else
                                            {
                                                savelvl[0, x, y] = new Blocks.NumberBlock(blocks[1, x, y].Id == 104 ? 165 : 214, coins);
                                            }
                                        }
                                        if (list.Count > 0)
                                        {
                                            foreach (KeyValuePair<int, int> kvp in list)
                                            {

                                                if (blocks[1, x, y].Id == kvp.Key)
                                                {
                                                    if (Blocks.IsType(kvp.Value, Blocks.BlockType.Normal))
                                                    {
                                                        savelvl[0, x, y] = new Blocks.Block(kvp.Value);
                                                    }
                                                }
                                                if (blocks[0, x, y].Id == kvp.Key)
                                                {
                                                    if (Blocks.IsType(kvp.Value, Blocks.BlockType.Normal))
                                                    {
                                                        savelvl[1, x, y] = new Blocks.Block(kvp.Value);
                                                    }
                                                }
                                            }
                                        }

                                    }


                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine(e);
                                }

                            }
                        }

                        var path = $"{Directory.GetCurrentDirectory()}\\worlds\\{clearillegal(savelvl.WorldName.Replace(" ", "_"), "")}_-_{savelvl.OwnerName}.eelvl";
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            savelvl.Save(fs);
                        }
                        if (rtxtbLog.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                rtxtbLog.AppendText($"Succesfully converted {savelvl.WorldName} by {savelvl.OwnerName}\n");
                            });
                        }

                        client.Socket.Abort();


                    }


                };
                await connection.SendAsync(MessageType.Init, 0);


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server returned status code '500'"))
                {
                    rtxtbLog.AppendText("Token expired, or wrong data!\n");
                }
                else
                {
                    rtxtbLog.AppendText($"{ex.Message}\n");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("worlds")) Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\worlds");
        }

        private void lvRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRooms.SelectedIndices.Count != 0)
            {
                txtbRoom.Text = lvRooms.SelectedItems[0].Name.Replace("rooms_", "");
            }
        }

        private async void btnOwn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbToken.Text))
            {
                lvRooms.Items.Clear();
                try
                {
                    var client = new EEUniverse.Library.Client(txtbToken.Text);
                    client.MaxBuffer = 1024 * 250;
                    await client.ConnectAsync();
                    client.OnDisconnect += (s2, m2) =>
                    {
                    };
                    client.OnMessage += (s1, m1) =>
                    {

                        if (m1.Type == MessageType.SelfInfo)
                        {

                            for (int i = 0; i < m1.GetInt(4); i++)
                            {
                                int incr = 5 + i;
                                var value = ((MessageObject)m1[incr++]);
                                if (lvRooms.InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        ListViewItem item = new ListViewItem();
                                        item.Name = $"rooms_{value.GetString("i")}";
                                        item.Text = value.GetString("n");
                                        item.SubItems.Add(value.GetInt("p").ToString());
                                        item.SubItems.Add(value.GetInt("v") == 1 ? "False" : "True");
                                        lvRooms.Items.Add(item);
                                    });
                                }
                            }
                            client.Socket.Abort();
                        }
                    };

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The server returned status code '500'"))
                    {
                        rtxtbLog.AppendText("Token expired, or wrong data!\n");
                    }
                    else
                    {
                        rtxtbLog.AppendText($"{ex.Message}\n");
                    }
                }
            }
        }

        private string clearillegal(string filename, string replaceChar)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar);
        }

        private void btnCB_Click(object sender, EventArgs e)
        {
            txtbToken.Text = Clipboard.GetText();
        }
    }
    public class myrooms
    {
        public string? roomname { get; set; }
        public int plays { get; set; }
        public bool visible { get; set; }
    }
}