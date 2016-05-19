using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowMuchInk_GUI
{
    class Options
    {
        public static decimal size1 = 0;
        public static decimal size2 = 0;
        public enum PaperSize { dinA4, dinA5, cm10xcm15, costum };
        public static String[] getDimensions(PaperSize p) {
            String[] ret = new String[2];
            switch(p) {
                case PaperSize.dinA4:
                    ret[0] = "210mm";
                    ret[1] = "297mm";
                    break;
                case PaperSize.dinA5:
                    ret[0] = "148mm";
                    ret[1] = "210mm";
                    break;
                case PaperSize.cm10xcm15:
                    ret[0] = "100mm";
                    ret[1] = "150mm";
                    break;
                default:
                    ret[0] = "";
                    ret[1] = "";
                    break;
            }
            return ret;
        }
        public static PaperSize getSize(String[] s)
        {
            try
            {
                String s1 = s[0].Replace(" ", "");
                String s2 = s[1].Replace(" ", "");
                decimal i1 = 0;
                decimal i2 = 0;
                if (s1.EndsWith("mm"))
                {
                    i1 = Decimal.Parse(s1.Replace("mm", ""));
                    goto Mark1;
                }
                if (s1.EndsWith("cm"))
                {
                    i1 = Decimal.Parse(s1.Replace("cm", "")) * 10;
                    goto Mark1;
                }
                if (s1.EndsWith("dm"))
                {
                    i1 = Decimal.Parse(s1.Replace("dm", "")) * 100;
                    goto Mark1;
                }
                if (s1.EndsWith("m"))
                {
                    i1 = Decimal.Parse(s1.Replace("m", "")) * 1000;
                    goto Mark1;
                }
                Mark1:
                size1 = i1;
                size2 = i2;
                if (s2.EndsWith("mm"))
                {
                    i2 = Decimal.Parse(s2.Replace("mm", ""));
                    goto Mark2;
                }
                if (s2.EndsWith("cm"))
                {
                    i2 = Decimal.Parse(s2.Replace("cm", "")) * 10;
                    goto Mark2;
                }
                if (s2.EndsWith("dm"))
                {
                    i2 = Decimal.Parse(s2.Replace("dm", "")) * 100;
                    goto Mark2;
                }
                if (s2.EndsWith("m"))
                {
                    i2 = Decimal.Parse(s2.Replace("m", "")) * 1000;
                    goto Mark2;
                }
                Mark2:
                if ((i1 == 210) & (i2 == 297))
                {
                    return PaperSize.dinA4;
                }
                else
                {
                    if ((i2 == 210) & (i1 == 297))
                    {
                        return PaperSize.dinA4;
                    }
                }
                if ((i1 == 148) & (i2 == 210))
                {
                    return PaperSize.dinA5;
                }
                else
                {
                    if ((i2 == 148) & (i1 == 210))
                    {
                        return PaperSize.dinA5;
                    }
                }
                if ((i1 == 100) & (i2 == 150))
                {
                    return PaperSize.cm10xcm15;
                }
                else
                {
                    if ((i2 == 100) & (i1 == 150))
                    {
                        return PaperSize.cm10xcm15;
                    }
                }
                //Markk
                s1 = s[1].Replace(" ", "");
                s2 = s[0].Replace(" ", "");
                if (s1.EndsWith("mm"))
                {
                    i1 = Decimal.Parse(s1.Replace("mm", ""));
                    goto Mark3;
                }
                if (s1.EndsWith("cm"))
                {
                    i1 = Decimal.Parse(s1.Replace("mm", "")) * 10;
                    goto Mark3;
                }
                if (s1.EndsWith("dm"))
                {
                    i1 = Decimal.Parse(s1.Replace("mm", "")) * 100;
                    goto Mark3;
                }
                if (s1.EndsWith("m"))
                {
                    i1 = Decimal.Parse(s1.Replace("mm", "")) * 1000;
                    goto Mark3;
                }
            Mark3:
                if (s2.EndsWith("mm"))
                {
                    i2 = Decimal.Parse(s2.Replace("mm", ""));
                    goto Mark4;
                }
                if (s2.EndsWith("cm"))
                {
                    i2 = Decimal.Parse(s2.Replace("mm", "")) * 10;
                    goto Mark4;
                }
                if (s2.EndsWith("dm"))
                {
                    i2 = Decimal.Parse(s2.Replace("mm", "")) * 100;
                    goto Mark4;
                }
                if (s2.EndsWith("m"))
                {
                    i2 = Decimal.Parse(s2.Replace("mm", "")) * 1000;
                    goto Mark4;
                }
            Mark4:
                if ((i1 == 210) & (i2 == 297))
                {
                    return PaperSize.dinA4;
                }
                else
                {
                    if ((i2 == 210) & (i1 == 297))
                    {
                        return PaperSize.dinA4;
                    }
                }
                if ((i1 == 148) & (i2 == 210))
                {
                    return PaperSize.dinA5;
                }
                else
                {
                    if ((i2 == 148) & (i1 == 210))
                    {
                        return PaperSize.dinA5;
                    }
                }
                if ((i1 == 100) & (i2 == 150))
                {
                    return PaperSize.cm10xcm15;
                }
                else
                {
                    if ((i2 == 100) & (i1 == 150))
                    {
                        return PaperSize.cm10xcm15;
                    }
                }
                return PaperSize.costum;
            }
            catch
            {
                return PaperSize.costum;
            }
        }
        public static decimal getDivisor() {
            return (210M*279M)/(size1*size2);
        }
    }
}
