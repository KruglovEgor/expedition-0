using UnityEngine;
using UnityEngine.UI;

namespace Expedition0.Tasks
{
    /// <summary>
    /// Простой скрипт для отображения результата бинарных операций в троичной логике
    /// Отображает результат AST в виде спрайтов чисел 0, 1, 2
    /// </summary>
    public class Type3ResultDisplay : MonoBehaviour
    {
        [Header("Result Display Settings")]
        [SerializeField] private Type3TaskLoader taskLoader; // Ссылка на загрузчик заданий
        [SerializeField] private Image resultImage; // UI Image для отображения результата
        
        [Header("Result Sprites")]
        [SerializeField] private Sprite sprite0; // Спрайт для значения 0 (False)
        [SerializeField] private Sprite sprite1; // Спрайт для значения 1 (Neutral)  
        [SerializeField] private Sprite sprite2; // Спрайт для значения 2 (True)
        
        [Header("Auto Update")]
        [SerializeField] private bool autoUpdate = true; // Автоматически обновлять результат
        [SerializeField] private float updateInterval = 0.1f; // Интервал обновления в секундах
        

        
        private Trit? currentResult; // Текущий результат
        private float lastUpdateTime;
        
        public Trit? CurrentResult => currentResult;

        private void Start()
        {
            // Автоматически найти taskLoader если не назначен
            if (taskLoader == null)
            {
                taskLoader = FindObjectOfType<Type3TaskLoader>();
                if (taskLoader == null)
                {
                    Debug.LogWarning("Type3ResultDisplay: TaskLoader not found! Please assign it manually.");
                }
            }
            
            // Автоматически найти resultImage если не назначен
            if (resultImage == null)
            {
                resultImage = GetComponent<Image>();
                if (resultImage == null)
                {
                    Debug.LogWarning("Type3ResultDisplay: Image component not found! Please assign resultImage or add Image component.");
                }
            }
            
            // Первоначальное обновление
            UpdateResult();
        }

        private void Update()
        {
            if (autoUpdate && Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateResult();
                lastUpdateTime = Time.time;
            }
        }

        public void UpdateResult()
        {
            if (taskLoader == null)
            {
                Debug.LogWarning("Type3ResultDisplay: TaskLoader is not assigned!");
                DisplayError();
                return;
            }

            ASTNode rootNode = taskLoader.GetRootNode();
            if (rootNode == null)
            {
                Debug.LogWarning("Type3ResultDisplay: Root node is null!");
                DisplayError();
                return;
            }

            try
            {
                // Вычисляем результат AST
                Trit result = SolutionAST.Solution(rootNode);
                
                // Проверяем, изменился ли результат
                if (currentResult != result)
                {
                    Debug.Log($"Type3ResultDisplay: Result changed from {currentResult} to {result}");
                    currentResult = result;
                    DisplayResult(result);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Type3ResultDisplay: Error evaluating AST: {e.Message}");
                currentResult = null;
                DisplayError();
            }
        }

        /// <summary>
        /// Отображает результат вычисления
        /// </summary>
        private void DisplayResult(Trit result)
        {
            if (resultImage == null)
            {
                Debug.LogWarning("Type3ResultDisplay: Result Image is not assigned!");
                return;
            }

            Sprite targetSprite = GetResultSprite(result);
            if (targetSprite != null)
            {
                resultImage.sprite = targetSprite;
                resultImage.enabled = true; // Показываем Image когда есть спрайт
                Debug.Log($"Type3ResultDisplay: Displayed sprite result: {result} ({result.ToInt()})");
            }
            else
            {
                // Скрываем Image когда нет подходящего спрайта
                resultImage.enabled = false;
                Debug.LogWarning($"Type3ResultDisplay: No sprite found for result {result}, hiding image");
            }
        }

        /// <summary>
        /// Отображает ошибку
        /// </summary>
        private void DisplayError()
        {
            if (resultImage != null)
            {
                // Скрываем Image при ошибке
                resultImage.enabled = false;
                Debug.Log("Type3ResultDisplay: Error occurred, hiding image");
            }
        }

        /// <summary>
        /// Получает спрайт для результата
        /// </summary>
        private Sprite GetResultSprite(Trit result)
        {
            switch (result.ToInt())
            {
                case 0: return sprite0; // False
                case 1: return sprite1; // Neutral
                case 2: return sprite2; // True
                default: return null;
            }
        }



        /// <summary>
        /// Принудительно обновляет результат (для вызова из других скриптов)
        /// </summary>
        public void ForceUpdate()
        {
            UpdateResult();
        }

        /// <summary>
        /// Принудительно обновляет результат (альтернативное имя для совместимости)
        /// </summary>
        public void ForceUpdateResult()
        {
            UpdateResult();
        }

        /// <summary>
        /// Устанавливает TaskLoader для получения AST
        /// </summary>
        public void SetTaskLoader(Type3TaskLoader loader)
        {
            taskLoader = loader;
            Debug.Log("Type3ResultDisplay: TaskLoader assigned");
        }

        /// <summary>
        /// Устанавливает спрайты для результатов
        /// </summary>
        public void SetResultSprites(Sprite s0, Sprite s1, Sprite s2)
        {
            sprite0 = s0;
            sprite1 = s1;
            sprite2 = s2;
            Debug.Log("Type3ResultDisplay: Result sprites assigned");
        }

        // Методы для тестирования в инспекторе
        [ContextMenu("Force Update Result")]
        public void TestForceUpdate()
        {
            UpdateResult();
        }

        [ContextMenu("Test Display 0")]
        public void TestDisplay0()
        {
            DisplayResult(Trit.False);
        }

        [ContextMenu("Test Display 1")]
        public void TestDisplay1()
        {
            DisplayResult(Trit.Neutral);
        }

        [ContextMenu("Test Display 2")]
        public void TestDisplay2()
        {
            DisplayResult(Trit.True);
        }

        [ContextMenu("Test Display Error")]
        public void TestDisplayError()
        {
            DisplayError();
        }
    }
}