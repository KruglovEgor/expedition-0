namespace Expedition0.Tasks
{

    public static class Create3TypeTasks
    {

        public static ASTTemplate CreateAndOrNeutralXY()
        {
            return Type3TaskTemplateFactory.CreateTripleLeftAssoc(
                innerOperator: Operator.OR,
                outerOperator: Operator.AND,
                answer: Trit.True,
                lockOperators: true,
                value1: Trit.Neutral,  // Заблокировано на 1
                value2: null,          // X - свободно
                value3: null           // Y - свободно
            );
        }

        public static ASTTemplate CreateOrXAndFalseY()
        {
            return Type3TaskTemplateFactory.CreateTripleRightAssoc(
                outerOperator: Operator.OR,
                innerOperator: Operator.AND,
                answer: Trit.Neutral,
                lockOperators: true,
                value1: null,          // X - свободно
                value2: Trit.False,    // Заблокировано на 0
                value3: null           // Y - свободно
            );
        }

        public static ASTTemplate CreateComplexXorAndOr()
        {
            return Type3TaskTemplateFactory.CreateComplexBinary(
                leftOperator: Operator.AND,
                rightOperator: Operator.OR,
                rootOperator: Operator.XOR,
                answer: Trit.False,
                lockOperators: true,
                value1: null,          // X - свободно
                value2: null,          // Y - свободно
                value3: Trit.Neutral,  // Заблокировано на 1
                value4: null           // Z - свободно
            );
        }
 
        public static ASTTemplate CreateNotX()
        {
            return Type3TaskTemplateFactory.CreateUnary(
                answer: Trit.True,
                lockOperator: true,
                valueLocked: null      // X - свободно
            );
        }

        public static ASTTemplate CreateNotXAndY()
        {
            // Создаем значения
            var xValue = new ValueSlotNode(); // X - свободно
            var yValue = new ValueSlotNode(); // Y - свободно
            
            // Создаем операторы
            var notOperator = new OperatorSlotNode(xValue, null); // NOT(X)
            notOperator.LockOperator(Operator.NOT);
            
            var andOperator = new OperatorSlotNode(notOperator, yValue); // AND(NOT(X), Y)
            // andOperator.LockOperator(Operator.AND);
            
            // Создаем списки слотов в правильном порядке для сцены:
            // Значения: X (слот 0), Y (слот 1)
            var valueSlots = new System.Collections.Generic.List<ValueSlotNode> { xValue, yValue };
            
            // Операторы: NOT (слот 0), AND (слот 1)
            var operatorSlots = new System.Collections.Generic.List<OperatorSlotNode> { notOperator, andOperator };
            
            // Используем конструктор с ручным порядком слотов
            return new ASTTemplate(andOperator, Trit.Neutral, valueSlots, operatorSlots);
        }

        public static ASTTemplate CreateImplyXYUnlocked()
        {
            return Type3TaskTemplateFactory.CreateBinary(
                predefinedOperator: Operator.IMPLY,
                answer: Trit.Neutral,
                lockOperator: false,   // Оператор разблокирован!
                leftLocked: null,      // X - свободно
                rightLocked: null      // Y - свободно
            );
        }

        public static ASTTemplate CreateFindOperator()
        {
            return Type3TaskTemplateFactory.CreateBinary(
                predefinedOperator: Operator.OR, // Начальное значение (будет изменено игроком)
                answer: Trit.Neutral,
                lockOperator: false,
                leftLocked: Trit.True,   // Заблокировано на 2
                rightLocked: Trit.False  // Заблокировано на 0
            );
        }

        public static ASTTemplate CreateCalculateResult()
        {
            return Type3TaskTemplateFactory.CreateComplexBinary(
                leftOperator: Operator.OR,
                rightOperator: Operator.XOR,
                rootOperator: Operator.AND,
                answer: Trit.Neutral, // Это нужно будет вычислить
                lockOperators: true,
                value1: null,          // X - свободно
                value2: Trit.Neutral,  // Заблокировано на 1
                value3: null,          // Y - свободно
                value4: Trit.True      // Заблокировано на 2
            );
        }

        public static ASTTemplate CreateXAndYOrZ()
        {
            // Создаем значения - используем все 3 слота как в сцене
            var xValue = new ValueSlotNode(); // X - свободно
            var yValue = new ValueSlotNode(); // Y - заблокировано
            var zValue = new ValueSlotNode(); // Z - заблокировано
            
            // Блокируем Y и Z на определенных значениях
            yValue.LockValue(Trit.True);   // Y = 2 (True)
            zValue.LockValue(Trit.False);  // Z = 0 (False)
            
            // Создаем операторы
            var andOperator = new OperatorSlotNode(xValue, yValue); // AND(X, Y)
            andOperator.LockOperator(Operator.AND);
            
            var orOperator = new OperatorSlotNode(andOperator, zValue); // OR(AND(X, Y), Z)
            orOperator.LockOperator(Operator.OR);
            
            // Создаем списки слотов в правильном порядке для сцены:
            // Значения: X (слот 0), Y (слот 1), Z (слот 2)
            var valueSlots = new System.Collections.Generic.List<ValueSlotNode> { xValue, yValue, zValue };
            
            // Операторы: AND (слот 0), OR (слот 1)
            var operatorSlots = new System.Collections.Generic.List<OperatorSlotNode> { andOperator, orOperator };
            
            // Вычисляем правильный ответ для X=True: (True AND True) OR False = True OR False = True
            return new ASTTemplate(orOperator, Trit.True, valueSlots, operatorSlots);
        }

        public static ASTTemplate CreateNotXorXY()
        {
            // Создаем значения
            var xValue = new ValueSlotNode(); // X - заблокирован
            var yValue = new ValueSlotNode(); // Y - свободно
            
            // Блокируем X на определенном значении
            xValue.LockValue(Trit.Neutral); // X = 1 (Neutral)
            
            // Создаем операторы
            var xorOperator = new OperatorSlotNode(xValue, yValue); // XOR(X, Y)
            // XOR не заблокирован - игрок может его менять
            xorOperator.SetOperator(Operator.XOR);
            
            var notOperator = new OperatorSlotNode(xorOperator, null); // NOT(XOR(X, Y))
            notOperator.LockOperator(Operator.NOT); // NOT заблокирован
            
            // Создаем списки слотов в правильном порядке для сцены:
            // Значения: X (слот 0), Y (слот 1)
            var valueSlots = new System.Collections.Generic.List<ValueSlotNode> { xValue, yValue };
            
            // Операторы: XOR (слот 0), NOT (слот 1)
            var operatorSlots = new System.Collections.Generic.List<OperatorSlotNode> { xorOperator, notOperator };
            
            // Вычисляем правильный ответ для X=True, Y=False: NOT(True XOR False) = NOT(True) = False
            // Но поскольку Y свободно, игрок может найти разные решения
            return new ASTTemplate(notOperator, Trit.False, valueSlots, operatorSlots);
        }

        public static bool Check(ASTTemplate template)
        {
            var lhs = SolutionAST.Solution(template.Root);
            return lhs == template.Answer;
        }

        public static Trit CalculateResult(ASTTemplate template)
        {
            return SolutionAST.Solution(template.Root);
        }
    }
}