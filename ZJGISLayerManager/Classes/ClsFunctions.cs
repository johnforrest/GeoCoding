using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;
using Microsoft.VisualBasic;

namespace ZJGISLayerManager
{
    static class ClsFunctions
    {
        public static ISymbol GetASymbolBySymbolType(string SymbolType, IColor aColor)
        {
            if (aColor == null)
            {
                IRgbColor pRgbColor;
                aColor = new RgbColorClass();
                pRgbColor = (IRgbColor)aColor;
                pRgbColor.RGB = Information.RGB((int)(VBMath.Rnd(1) * 255), (int)(VBMath.Rnd(1) * 255), (int)(VBMath.Rnd(1) * 255));
            }

            switch (SymbolType)
            {
                case "面状地物符号":
                    ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                    pFillSymbol.Color = aColor;
                    return (ISymbol)pFillSymbol;
                case "线状地物符号":
                    ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
                    pLineSymbol.Width = 1;
                    pLineSymbol.Color = aColor;
                    return (ISymbol)pLineSymbol;
                case "点状地物符号":
                    IMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                    pMarkerSymbol.Color = aColor;
                    return (ISymbol)pMarkerSymbol;
                default:
                    return null;
            }
        }
    }
}
