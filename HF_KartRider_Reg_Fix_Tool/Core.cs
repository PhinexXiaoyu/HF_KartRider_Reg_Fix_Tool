using System;
using System.Management;
using Microsoft.Win32;
using System.Windows.Forms;

namespace HF_KartRider_Reg_Fix_Tool
{
    static class Core
    {
        //修复标志
        public static bool flag = false;

        //获取系统位宽
        public static string GetOS3264()
        {
            ConnectionOptions oConn = new ConnectionOptions();
            ManagementScope oMs = new ManagementScope(@"\\localhost", oConn);
            ObjectQuery oQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oReturnCollection = oSearcher.Get();
            string addresswidth = null;

            foreach (ManagementObject oReturn in oReturnCollection)
                addresswidth = oReturn["AddressWidth"].ToString();

            return addresswidth;
        }

        //修复32位系统注册表
        public static void WriteReg32(string fileName)
        {
            try
            {
                RegistryKey k = openSoftware(@"SOFTWARE");
                writeReg(fileName, k);
                flag = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        //修复64位系统注册表
        public static void WriteReg64(string fileName)
        {
            try
            {
                RegistryKey k = openSoftware(@"SOFTWARE\Wow6432Node");
                writeReg(fileName, k);
                flag = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        //打开要修复的键
        public static RegistryKey openSoftware(string str)
        {
            RegistryKey key = Registry.LocalMachine;
            return key.OpenSubKey(str, true);
        }

        //韩服跑跑相关
        public static void writeReg(string fileName, RegistryKey key)
        {
            char[] temp = fileName.ToCharArray();
            char[] temp1 = new char[fileName.Length];
            for (int i = 0; i < fileName.Length - 14; i++)
                temp1[i] = temp[i];
            string path = new string(temp1);

            RegistryKey kart = key.CreateSubKey(@"Nexon\KartRider\M01");
            kart.SetValue("Executable", fileName);
            kart.SetValue("RootPath", path);
            kart.SetValue("Icon",fileName + @", 0");
            kart.SetValue(@"{B91A5233-AC0B-4197-9359-834B3583C8F4}", 10, RegistryValueKind.DWord);
            key.Close();
        }
    }
}
