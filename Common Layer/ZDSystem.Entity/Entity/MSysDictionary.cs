using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

      ///<summary>
      ///实体：差异
      ///</summary>
     [ODXmlConfig(EntityConfigType.Xml,"","~/config/econfig/SysDictionary.xml")]
    public class MSysDictionary:EntityBase
    {    

                private decimal? _Dicid;
        private string _Name;
        private string _Value;
        private string _Type;
        private decimal? _Short;


                 ///<summary>
        ///编号
        ///</summary>
        public decimal? Dicid{get { return _Dicid; }set { _Dicid= value; }}


        ///<summary>
        ///名称
        ///</summary>
        public string Name{get { return _Name; }set { _Name= value; }}


        ///<summary>
        ///值
        ///</summary>
        public string Value{get { return _Value; }set { _Value= value; }}


        ///<summary>
        ///类型
        ///</summary>
        public string Type{get { return _Type; }set { _Type= value; }}


        ///<summary>
        ///顺序
        ///</summary>
        public decimal? Short{get { return _Short; }set { _Short= value; }}




    }
}