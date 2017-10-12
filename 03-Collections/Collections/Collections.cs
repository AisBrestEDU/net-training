using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Collections.Tasks {

    /// <summary>
    ///  Tree node item 
    /// </summary>
    /// <typeparam name="T">the type of tree node data</typeparam>
    public interface ITreeNode<T> {
        T Data { get; set; }                             // Custom data
        IEnumerable<ITreeNode<T>> Children { get; set; } // List of childrens
    }


    public class Task {

        /// <summary> Generate the Fibonacci sequence f(x) = f(x-1)+f(x-2) </summary>
        /// <param name="count">the size of a required sequence</param>
        /// <returns>
        ///   Returns the Fibonacci sequence of required count
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0</exception>
        /// <example>
        ///   0 => { }  
        ///   1 => { 1 }    
        ///   2 => { 1, 1 }
        ///   12 => { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144 }
        /// </example>
        public static IEnumerable<int> GetFibonacciSequence(int count) {
            // TODO : Implement Fibonacci sequence generator
            //throw new NotImplementedException();

            if (count < 0) throw new ArgumentException();
            if (count == 0) yield break;

            int a = 1; int b = 0;

            yield return a;

            while (--count > 0)
            {
                yield return a += b;
                b = a - b;
             }
          
        }

        /// <summary>
        ///    Parses the input string sequence into words
        /// </summary>
        /// <param name="reader">input string sequence</param>
        /// <returns>
        ///   The enumerable of all words from input string sequence. 
        /// </returns>
        /// <exception cref="System.ArgumentNullException">reader is null</exception>
        /// <example>
        ///  "TextReader is the abstract base class of StreamReader and StringReader, which ..." => 
        ///   {"TextReader","is","the","abstract","base","class","of","StreamReader","and","StringReader","which",...}
        /// </example>
        public static IEnumerable<string> Tokenize(TextReader reader) {
            char[] delimeters = new[] { ',', ' ', '.', '\t', '\n' };
            // TODO : Implement the tokenizer
            //throw new NotImplementedException();

            if (reader == null) throw new ArgumentNullException();

            while (true)
            {
                var str = reader.ReadLine();

                if (str == null) break;

                var wordArr = str.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var w in wordArr) yield return w;
            }
            
        }



        /// <summary>
        ///   Traverses a tree using the depth-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in depth-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  6  7
        ///                  / \     \
        ///                 3   4     8
        ///                     |
        ///                     5   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> DepthTraversalTree<T>(ITreeNode<T> root) {
            // TODO : Implement the tree depth traversal algorithm
            //throw new NotImplementedException();

            if (root == null) throw new ArgumentNullException();

            var buff = new Stack<ITreeNode<T>>();
            buff.Push(root);

            while (buff.Count > 0)
            {
                var node = buff.Pop();
                foreach (var n in (node.Children ?? new List<ITreeNode<T>>()).Reverse()) buff.Push(n);
                yield return node.Data;
            }

        }

        /// <summary>
        ///   Traverses a tree using the width-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in width-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  3  4
        ///                  / \     \
        ///                 5   6     7
        ///                     |
        ///                     8   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> WidthTraversalTree<T>(ITreeNode<T> root) {
            // TODO : Implement the tree width traversal algorithm
            //throw new NotImplementedException();

            if (root == null) throw new ArgumentNullException();

            var buff = new Queue<ITreeNode<T>>();
            buff.Enqueue(root);

            while(buff.Count>0)
            {
                var node = buff.Dequeue();
                foreach (var n in node.Children ?? new List<ITreeNode<T>>()) buff.Enqueue(n);
                yield return node.Data;
            }

        }



        /// <summary>
        ///   Generates all permutations of specified length from source array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">source array</param>
        /// <param name="count">permutation length</param>
        /// <returns>
        ///    All permuations of specified length
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0 or greater then the source length</exception>
        /// <example>
        ///   source = { 1,2,3,4 }, count=1 => {{1},{2},{3},{4}}
        ///   source = { 1,2,3,4 }, count=2 => {{1,2},{1,3},{1,4},{2,3},{2,4},{3,4}}
        ///   source = { 1,2,3,4 }, count=3 => {{1,2,3},{1,2,4},{1,3,4},{2,3,4}}
        ///   source = { 1,2,3,4 }, count=4 => {{1,2,3,4}}
        ///   source = { 1,2,3,4 }, count=5 => ArgumentOutOfRangeException
        /// </example>
        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count) {
            // TODO : Implement GenerateAllPermutations method
            //throw new NotImplementedException();

            if (count > source.Count()) throw new ArgumentOutOfRangeException();
            if (count == 0) return new List<T[]> { };

            var result = new List<T[]>();
            var sourceM = new List<T[]>();
            var buff = new List<T[]>();
            foreach (var t in source) { result.Add(new T[] { t }); sourceM.Add(new T[] { t }); }

            if (count == 1) return result;

            //повторяем пока неполучим комбинации нужной длинны
            for (int i = 1; i < count; i++)
            {
                //проходим по всему стартоваму набору и строим комбинации=>для каждого иследуемого элимента со всеми элиментами 
                //"больше (больше=>ближе к концу массива)" чем последний элимент в иследуемом элименте.
                for (int j = 0; j < result.Count; j++)
                    //ка присваиваеться индекс элимента который "больше" чем последний элимент в иследуемом элименте
                    for (int k = source.ToList().FindIndex(x => x.Equals(result[j].Last())) + 1; k < sourceM.Count; k++)
                        buff.Add(result[j].Concat<T>(sourceM[k]).ToArray());

                //удаляем комбинации которые меньше необходимой(для текущей итерации) дленны
                for (int j = 0; j < buff.Count; j++) if (buff[j].Length != i + 1) buff.RemoveAt(j);

                result.Clear(); result.AddRange(buff); buff.Clear();
            }

            return result;
        }
    }



    public static class DictionaryExtentions {
        
        /// <summary>
        ///    Gets a value from the dictionary cache or build new value
        /// </summary>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        /// <param name="dictionary">source dictionary</param>
        /// <param name="key">key</param>
        /// <param name="builder">builder function to build new value if key does not exist</param>
        /// <returns>
        ///   Returns a value assosiated with the specified key from the dictionary cache. 
        ///   If key does not exist than builds a new value using specifyed builder, puts the result into the cache 
        ///   and returns the result.
        /// </returns>
        /// <example>
        ///   IDictionary<int, Person> cache = new SortedDictionary<int, Person>();
        ///   Person value = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should return a loaded Person and put it into the cache
        ///   Person cached = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should get a Person from the cache
        /// </example>
        public static TValue GetOrBuildValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> builder) {
            // TODO : Implement GetOrBuildValue method for cache
            throw new NotImplementedException();
        }

    }
}
