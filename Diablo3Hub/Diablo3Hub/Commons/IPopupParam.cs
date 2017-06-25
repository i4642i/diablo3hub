﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo3Hub.Commons
{
    /// <summary>
    /// 팝업에 파라메터를 전달 할 경우 인터페이스
    /// </summary>
    internal interface IPopupParam
    {
        void SetParam(object param);
    }
}
