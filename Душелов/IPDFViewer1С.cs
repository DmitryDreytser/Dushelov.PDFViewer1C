// Decompiled with JetBrains decompiler
// Type: Душелов.IPDFViewer1С
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.Runtime.InteropServices;

namespace Душелов
{
    [Guid("8380253E-C21D-40e7-B20D-43B18354C694")]
    public interface IPDFViewer1С : IDisposable
    {
        bool ВидимостьПанелиИнструментов { get; set; }

        bool ВидимостьОкнаЗакладок { get; set; }

        int ТекущаяСтраница { get; set; }

        int КоличествоСтраниц { get; }

        double Масштаб { get; set; }

        void Закрыть();

        bool ЗагрузитьДокумент(string Файл);

        int Найти(string СтрокаПоиска, bool СНачала, bool ТочноеСовпадение, bool ПоВсемуДокументу, bool Следующий, bool ПоискВверх);

    }
}
