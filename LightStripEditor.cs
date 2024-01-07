using NightDriver;
using System.Net.NetworkInformation;
using System.Text;

namespace NightDriverDesktop
{
    public partial class LightStripEditor : Form
    {
        public LightStripEditor()
        {
            InitializeComponent();
        }


        internal enum DialogOperatingMode
        {
            New,
            Edit
        }

        internal DialogOperatingMode DialogMode { get; set; }
        internal LightStrip StripToEdit { get; set; }

        private const int maxLedCount = 1000;

        private uint ledHeight { get; set; }
        private uint ledWidth { get; set; }
        private uint ledOffset { get; set; }
        private bool redGreenSwapped { get; set; }
        private byte ledStripChannel { get; set; }

        private bool IsPixelCountValid(string height, string width)
        {
            bool heightValid = false;
            bool widthValid = false;
            bool pixelCountValid = false;

            //keep this as an int so we can do the width test if we receive -1
            if (int.TryParse(height, out int heightValue))
            {
                heightValid = true;
            }

            //keep this as an int so we can do the height test if we receive -1
            if (int.TryParse(width, out int widthValue))
            {
                widthValid = true;
            }

            //we use -1 as width to test height and vice-versa for testng width, set them to positive 1 so the next tests pass.
            if (widthValue == -1 || heightValue == -1)
            {
                //we can safely set them both to 1 here because we're only testing if the width or height are numeric
                widthValue = 1;
                heightValue = 1;
            }

            //check if the value we received is negative or 0 (never been set)
            if (widthValue <= 0 || heightValue <= 0)
            {
                //if we get here and we still have a negative value then we've received inavlid input for a uint.
                return false;
            }

            if (widthValue * heightValue <= maxLedCount && widthValue * heightValue > 0)
            {
                pixelCountValid = true;
                ledHeight = (uint)heightValue;
                ledWidth = (uint)widthValue;
            }

            return heightValid && widthValid && pixelCountValid;
        }

        //Use a ping to see if there's anything at the specified host name or IP address
        private bool IsHostAlive(string hostNameOrIpAddress)
        {

            Ping pingTest = new();
            PingOptions options = new PingOptions();
            //No fair sending pings across the internet.  5 should keep it reasonably local, even on most campus networks.
            options.Ttl = 5;
            byte[] packetBytes = Encoding.ASCII.GetBytes("This is a test to see if your NightDriver instance IP is correct");
            byte successCount = 0;
            //Send 5 pings with 250 millisecond timeout.  If your latenciy is greater than 250ms you're going to have a bad time with really low FPS
            //This also makes the test complete in about 1.25 seconds so it doesn't feel like we're waiting forever.
            for (byte i = 1; i <= 5; i++)
            {
                PingReply reply = pingTest.Send(
                                            hostNameOrIpAddress,
                                            250,
                                            packetBytes,
                                            options);
                if (reply.Status == IPStatus.Success)
                {
                    successCount ++;
                }
            }
            
            if (successCount >= 5) 
            { 
                return true;
            } 
            else
            {
                return false;
            }
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            if (!IsPixelCountValid(textBoxHeight.Text, "-1"))
            {
                MessageBox.Show(
                    "Only positive integers are valid in this field",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                textBoxHeight.Text = string.Empty;
                return;
            }
        }

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            if (!IsPixelCountValid("-1", textBoxWidth.Text))
            {
                MessageBox.Show(
                    "Only positive integers are valid in this field",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                textBoxWidth.Text = string.Empty;
                return;
            }
        }

        private void textBoxPixelOffset_TextChanged(object sender, EventArgs e)
        {
            if (uint.TryParse(textBoxPixelOffset.Text, out uint offsetValue))
            {
                ledOffset = offsetValue;
            }
            else
            {
                MessageBox.Show(
                  "Only positive integers are valid in this field",
                  "Validation Error",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error
                  );
                textBoxWidth.Text = string.Empty;
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //check that we have a width and height
            if (!IsPixelCountValid(textBoxHeight.Text, textBoxWidth.Text))
            {
                MessageBox.Show(
                        String.Format("Pixel count is not valid. Ensure that the total number of LEDs is less than {0}", maxLedCount),
                        "Pixel Count Invalid",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                return;
            }

            //check that there's something in the hostname field
            if (textBoxHostName.Text.Trim() == string.Empty)
            {
                MessageBox.Show(
                    "Hostname or IP Address is required.",
                    "Hostname is Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //if the friendly name is blank, use the hostname in its place
            if (textBoxFriendlyName.Text == string.Empty)
            {
                textBoxFriendlyName.Text = textBoxHostName.Text.ToUpper();
            }

            //check if the NightDriver server specified is available, unless the skip network checkbox is set
            if (!IsHostAlive(textBoxHostName.Text.Trim()) && !checkBoxSkipNetTest.Checked)
            {
                MessageBox.Show(
                    "Host is not responding to pings, is too slow to respond or is too many hops away.\r\n\r\n" +
                    "Maximum ping time is 250ms\r\n\r\n" +
                    "Maximum route hops is 5",
                    "Unable to contact NightDriver Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            
            LightStrip strip = new LightStrip(
                hostName: textBoxHostName.Text,
                friendlyName: textBoxFriendlyName.Text,
                compressData: checkBoxCompression.Checked,
                width: ledWidth,
                height: ledHeight,
                offset: ledOffset,
                reversed: checkBoxReverseOrder.Checked,
                channel: ledStripChannel,
                redGreenSwap: redGreenSwapped
                );
            if (DialogMode == DialogOperatingMode.Edit)
            {
                strip.StripSite = this.StripToEdit.StripSite;
            }
            else
            {
                strip.StripSite = new Site(textBoxFriendlyName.Text.Replace(" ", "_"), ledWidth, ledHeight);
            }
            this.Tag = strip; 
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBoxStripType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DialogMode == DialogOperatingMode.Edit)
            {
                textBoxWidth.Text = this.StripToEdit.Width.ToString();
                textBoxHeight.Text = this.StripToEdit.Width.ToString();
            }
            switch (comboBoxStripType.SelectedItem)
            {
                case "Single Strip":
                    textBoxHeight.Text = "1";
                    if (DialogMode == DialogOperatingMode.New)
                    {
                        textBoxWidth.Text = "144";
                    }
                    textBoxHeight.Enabled = false;
                    textBoxWidth.Enabled = true;
                    break;
                case "LED Matrix":
                    textBoxHeight.Enabled = true;
                    textBoxWidth.Enabled = true;
                    if (DialogMode == DialogOperatingMode.New)
                    {
                        textBoxHeight.Text = "8";
                        textBoxWidth.Text = "8";
                    }
                    break;
                default:
                    break;
            }
        }

        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (byte.TryParse((string)comboBoxChannel.SelectedItem, out byte value))
            {
                ledStripChannel = value;
            }
            else
            {
                MessageBox.Show(
                      "Selected channel is not valid.",
                      "Invalid Channel Selected",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error
                      );
                return;
            }
        }

        private void comboBoxColorOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxColorOrder.SelectedItem)
            {
                case "RGB":
                    redGreenSwapped = true;
                    break;
                case "GRB":
                    redGreenSwapped = false;
                    break;
                default:
                    redGreenSwapped = false;
                    break;
            }
        }

        private void LightStripEditor_Load(object sender, EventArgs e)
        {
            if (this.DialogMode == DialogOperatingMode.New)
            {
                //set the default values for key variables and form state
                ledStripChannel = 0;
                ledHeight = 0;
                ledWidth = 0;
                textBoxWidth.Enabled = false;
                textBoxHeight.Enabled = false;
                comboBoxChannel.SelectedItem = "0";
                comboBoxColorOrder.SelectedItem = "GRB";
                redGreenSwapped = false;
            }
            else if (this.DialogMode == DialogOperatingMode.Edit && this.StripToEdit is null)
            {
                MessageBox.Show(
                    "No record selected to edit.",
                    "Make a selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                this.Close();
            }
            else if (this.DialogMode == DialogOperatingMode.Edit && this.StripToEdit is LightStrip)
            {
                //load the strip to edit
                ledStripChannel = this.StripToEdit.Channel;
                comboBoxChannel.SelectedItem = this.StripToEdit.Channel.ToString();
                ledHeight = this.StripToEdit.Height;
                ledWidth = this.StripToEdit.Width;
                textBoxFriendlyName.Text = this.StripToEdit.FriendlyName.ToString();
                textBoxHostName.Text = this.StripToEdit.HostName.ToString();
                checkBoxCompression.Checked = this.StripToEdit.CompressData;
                if (this.StripToEdit.RedGreenSwap)
                {
                    comboBoxColorOrder.SelectedItem = "RGB";
                }
                else
                {
                    comboBoxColorOrder.SelectedItem = "GRB";
                }
                if (ledHeight > 0)
                {
                    comboBoxStripType.SelectedItem = "Single Strip";
                }
                else
                {
                    comboBoxStripType.SelectedItem = "LED Matrix";
                }
            }
            else
            {
                MessageBox.Show(
                    "DIalog mode was not specified.",
                    "Error Calling LighStrip Editor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                this.Close();
            }
        }


    }
}
