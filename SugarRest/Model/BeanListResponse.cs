using System.Collections.Generic;

namespace SugarRest.Model
{
    public class BeanListResponse<TBean> : BeanListBase where TBean : new ()
    {
        public List<TBean> records { get; set; }
    }
}
