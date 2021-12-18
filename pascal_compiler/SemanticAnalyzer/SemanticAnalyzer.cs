/* Проверяет, не нарушены ли неформальные правила описания языка
* Вход: синтаксически проверенная программа
 * Выход: проверенная программа, семантические ошибки
 */

//ctype приводимость типов
//Таблица переменных


//ошибки: нет идентификатор, неприводимые типы

//смотря какой тип первой термы, придаем значение второй, если тип неизвестный
//Если дублируется, нужно сказать 

//не надо проверку инициализации пока
//Всего 1 таблица

//Проверки идентификатора: без writeln

using System;
using System.Collections.Generic;

namespace SemanticAnalyzer
{
    public enum EType
    {
        Integer,
        Real,
        String,
        Char,
        Boolean,
        NotDerivable
    }

    public struct DerividedTypeList
    {
        public EType DerividedToInt;
        public EType DerividedToReal;
        public EType DerividedToString;
        public EType DerividedToChar;
        public EType DerividedToBoolean;

        public DerividedTypeList(EType DerividedToInt, EType DerividedToReal, EType DerividedToString, EType DerividedToChar, EType DerividedToBoolean)
        {
            this.DerividedToInt = DerividedToInt;
            this.DerividedToReal = DerividedToReal;
            this.DerividedToString = DerividedToString;
            this.DerividedToChar = DerividedToChar;
            this.DerividedToBoolean = DerividedToBoolean;
        }
    }

    public struct DerividedBoolList
    {
        public bool DerividedToInt;
        public bool DerividedToReal;
        public bool DerividedToString;
        public  bool DerividedToChar;
        public bool DerividedToBoolean;

        public DerividedBoolList(bool DerividedToInt, bool DerividedToReal, bool DerividedToString, bool DerividedToChar, bool DerividedToBoolean)
        {
            this.DerividedToInt = DerividedToInt;
            this.DerividedToReal = DerividedToReal;
            this.DerividedToString = DerividedToString;
            this.DerividedToChar = DerividedToChar;
            this.DerividedToBoolean = DerividedToBoolean;
        }
    }

    //Сделать класс: объект имеет тип, структуру 


    public class Semantic
    {
       public Dictionary<string, EType> Types_Table = new Dictionary<string, EType>();

       public abstract class CType
       {
            public abstract DerividedTypeList ArithmeticDerivided { get; set; }
            public abstract DerividedTypeList AssignmentDerivided { get; set; }
            public abstract DerividedBoolList CompareDerivided { get; set; }
        }

        public class IntCast: CType
        {
            public override DerividedTypeList ArithmeticDerivided { get; set; }
            public override DerividedTypeList AssignmentDerivided { get; set; }
            public override DerividedBoolList CompareDerivided { get; set; }
            public IntCast()
            {
                ArithmeticDerivided = new DerividedTypeList(
                    DerividedToInt: EType.Integer,
                    DerividedToReal: EType.Real,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                //Типу класса присваивается следующий тип:
                AssignmentDerivided = new DerividedTypeList(
                    DerividedToInt: EType.Integer,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                CompareDerivided = new DerividedBoolList(
                    DerividedToInt: true,
                    DerividedToReal: true,
                    DerividedToString: false,
                    DerividedToChar: false,
                    DerividedToBoolean: false);
            }
        }

        public class RealCast : CType
        {
            public override DerividedTypeList ArithmeticDerivided { get; set; }
            public override DerividedTypeList AssignmentDerivided { get; set; }
            public override DerividedBoolList CompareDerivided { get; set; }
            public RealCast()
            {
                ArithmeticDerivided = new DerividedTypeList(
                    DerividedToInt: EType.Real,
                    DerividedToReal: EType.Real,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                AssignmentDerivided = new DerividedTypeList(
                    DerividedToInt: EType.Real,
                    DerividedToReal: EType.Real,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                CompareDerivided = new DerividedBoolList(
                    DerividedToInt: true,
                    DerividedToReal: true,
                    DerividedToString: false,
                    DerividedToChar: false,
                    DerividedToBoolean: false);
            }
        }

        public class StringCast : CType
        {
            public override DerividedTypeList ArithmeticDerivided { get; set; }
            public override DerividedTypeList AssignmentDerivided { get; set; }
            public override DerividedBoolList CompareDerivided { get; set; }
            public StringCast()
            {
                ArithmeticDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.String,
                    DerividedToChar: EType.String,
                    DerividedToBoolean: EType.NotDerivable);

                AssignmentDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.String,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                CompareDerivided = new DerividedBoolList(
                    DerividedToInt: false,
                    DerividedToReal: false,
                    DerividedToString: true,
                    DerividedToChar: true,
                    DerividedToBoolean: false);
            }
        }

        public class CharCast : CType
        {
            public override DerividedTypeList ArithmeticDerivided { get; set; }
            public override DerividedTypeList AssignmentDerivided { get; set; }
            public override DerividedBoolList CompareDerivided { get; set; }
            public CharCast()
            {
                ArithmeticDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.String,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.NotDerivable);

                AssignmentDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.Char,
                    DerividedToBoolean: EType.NotDerivable);

                CompareDerivided = new DerividedBoolList(
                    DerividedToInt: false,
                    DerividedToReal: false,
                    DerividedToString: true,
                    DerividedToChar: true,
                    DerividedToBoolean: false);
            }
        }

        public class BooleanCast : CType
        {
            public override DerividedTypeList ArithmeticDerivided { get; set; }
            public override DerividedTypeList AssignmentDerivided { get; set; }
            public override DerividedBoolList CompareDerivided { get; set; }
            public BooleanCast()
            {
                ArithmeticDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.Boolean);

                AssignmentDerivided = new DerividedTypeList(
                    DerividedToInt: EType.NotDerivable,
                    DerividedToReal: EType.NotDerivable,
                    DerividedToString: EType.NotDerivable,
                    DerividedToChar: EType.NotDerivable,
                    DerividedToBoolean: EType.Boolean);

                CompareDerivided = new DerividedBoolList(
                    DerividedToInt: false,
                    DerividedToReal: false,
                    DerividedToString: false,
                    DerividedToChar: false,
                    DerividedToBoolean: true);
            }
        }

        public bool isComparable(EType Left, EType Right)
        {
            switch (Left)
            {
                case EType.Integer:
                {
                        IntCast Type = new IntCast(); 
                        switch (Right)
                        {
                            case EType.Integer:
                            {
                                return Type.CompareDerivided.DerividedToInt;  
                            }
                            case EType.Real:
                            {
                                return Type.CompareDerivided.DerividedToReal;
                            }
                            case EType.String:
                            {
                                return Type.CompareDerivided.DerividedToString;
                            }
                            case EType.Char:
                            {
                                return Type.CompareDerivided.DerividedToChar;
                            }
                            case EType.Boolean:
                            {
                                return Type.CompareDerivided.DerividedToBoolean;
                            }
                        }
                        break;
                }
                case EType.Real:
                {
                    RealCast Type = new RealCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.CompareDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.CompareDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.CompareDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.CompareDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.CompareDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.String:
                {
                    StringCast Type = new StringCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                                return Type.CompareDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                                return Type.CompareDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                                return Type.CompareDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                                return Type.CompareDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                                return Type.CompareDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Char:
                {
                    CharCast Type = new CharCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.CompareDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.CompareDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.CompareDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.CompareDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.CompareDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Boolean:
                {
                    BooleanCast Type = new BooleanCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.CompareDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.CompareDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.CompareDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.CompareDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.CompareDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
            }
            Console.WriteLine("Not found type");
            return false;   
        }

        public EType IsAssignable(EType Left, EType Right)
        {
            switch (Left)
            {
                case EType.Integer:
                {
                    IntCast Type = new IntCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.AssignmentDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.AssignmentDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.AssignmentDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.AssignmentDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.AssignmentDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Real:
                {
                    RealCast Type = new RealCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.AssignmentDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.AssignmentDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.AssignmentDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.AssignmentDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.AssignmentDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.String:
                {
                    StringCast Type = new StringCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.AssignmentDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.AssignmentDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.AssignmentDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.AssignmentDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.AssignmentDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Char:
                {
                    CharCast Type = new CharCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.AssignmentDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.AssignmentDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.AssignmentDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.AssignmentDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.AssignmentDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Boolean:
                    {
                        BooleanCast Type = new BooleanCast();
                        switch (Right)
                        {
                            case EType.Integer:
                            {
                                return Type.AssignmentDerivided.DerividedToInt;
                            }
                            case EType.Real:
                            {
                                return Type.AssignmentDerivided.DerividedToReal;
                            }
                            case EType.String:
                            {
                                return Type.AssignmentDerivided.DerividedToString;
                            }
                            case EType.Char:
                            {
                                return Type.AssignmentDerivided.DerividedToChar;
                            }
                            case EType.Boolean:
                            {
                                return Type.AssignmentDerivided.DerividedToBoolean;
                            }
                        }
                        break;
                    }
                }
            Console.WriteLine("Not found type");
            return EType.NotDerivable;
        }

        public EType IsArithmeticDerivided(EType Left, EType Right)
        {
            switch (Left)
            {
                case EType.Integer:
                {
                    IntCast Type = new IntCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.ArithmeticDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.ArithmeticDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.ArithmeticDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.ArithmeticDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.ArithmeticDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Real:
                {
                    RealCast Type = new RealCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.ArithmeticDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.ArithmeticDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.ArithmeticDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.ArithmeticDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.ArithmeticDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.String:
                {
                    StringCast Type = new StringCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.ArithmeticDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.ArithmeticDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.ArithmeticDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.ArithmeticDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.ArithmeticDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Char:
                {
                    CharCast Type = new CharCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.ArithmeticDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.ArithmeticDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.ArithmeticDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.ArithmeticDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.ArithmeticDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
                case EType.Boolean:
                {
                    BooleanCast Type = new BooleanCast();
                    switch (Right)
                    {
                        case EType.Integer:
                        {
                            return Type.ArithmeticDerivided.DerividedToInt;
                        }
                        case EType.Real:
                        {
                            return Type.ArithmeticDerivided.DerividedToReal;
                        }
                        case EType.String:
                        {
                            return Type.ArithmeticDerivided.DerividedToString;
                        }
                        case EType.Char:
                        {
                            return Type.ArithmeticDerivided.DerividedToChar;
                        }
                        case EType.Boolean:
                        {
                            return Type.ArithmeticDerivided.DerividedToBoolean;
                        }
                    }
                    break;
                }
            }
            Console.WriteLine("Not found type");
            return EType.NotDerivable;
        }



        public bool AddIdent(string Ident_Name, string Type)
        {
            if (Types_Table.ContainsKey(Ident_Name))
            {
                Console.WriteLine("Словарь содержит идентификатор");
                return false;
            }
            else
            {
                switch(Type)
                {
                    case "integer":
                    {
                        Types_Table.Add(Ident_Name, EType.Integer);
                        break;
                    }
                    case "real":
                    {
                        Types_Table.Add(Ident_Name, EType.Real);
                        break;
                    }
                    case "string":
                    {
                        Types_Table.Add(Ident_Name, EType.String);
                        break;
                    }
                    case "char":
                    {
                        Types_Table.Add(Ident_Name, EType.Char);
                        break;
                    }
                    case "boolean":
                    {
                        Types_Table.Add(Ident_Name, EType.Boolean);
                        break;
                    }
                    default: return false;
                }
                return true;
            }
        }

        public EType ConvertToType(string raw_type)
        {
            switch (raw_type)
            {
               case "Integer":
                {
                    return EType.Integer;
                }
                case "Real":
                {
                    return EType.Real;
                }
                case "String":
                {
                    return EType.String;
                }
                case "Char":
                {
                    return EType.Char;
                }
                case "Boolean":
                {
                    return EType.Boolean;
                }
                default: return EType.NotDerivable;
            }
        }

        public string ConvertToString(EType rawtype)
        {
            switch(rawtype)
            {
                case EType.Integer:
                    {
                        return "Integer";
                    }
                case EType.Real:
                    {
                        return "Real";
                    }
                case EType.String:
                    {
                        return "String";
                    }
                case EType.Char:
                    {
                        return "Char";
                    }
                case EType.Boolean:
                    {
                        return "Boolean";
                    }
                default: return "";
            }
        }
    }


   // 
}

/*
 * enum EType {
  et_integer,
  et_string,
...
}
abstract class CType {
  bool isDerivedTo (CType b); // from ? на подумать
  EType myType
}
class CIntType : CType { ... }

class CCompiler {
  map<string, CTypes> availableTypes = new map<>({"integer", new CIntType()}, ...);
  CType deriveTo(CType left, CType right) { ... }

  ....

  CType simpleExpressionCommand() {
    var left = factor();
    while ...{
      var right = factor();
      if (left.isDerivedTo(right)) { left = deriveTo(left, right) } else { throw new Exception("не приводимые типы"); }
    }
  }

  ....
  void ifCommand() {
    accept(if_sy);
    accept(left_bracet_sy);
    var exprType = simpleExpressionCommand();
    accept(right_bracet_sy);
    if (!exprType.isDerivedTo(availableTypes['boolean']) throw new Exception("не приводимые типы");
  }
}
*/