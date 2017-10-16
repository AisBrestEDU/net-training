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
            int n1 = 0, n2 = 1, n = 0;
            List<int> result = new List<int>();
            result.Add(n2);
            int index = 2;

            if (count == 0) return new List<int>();
            if (count < 0) throw new ArgumentException();

            while (index <= count)
            {
                n = n1 + n2;
                n1 = n2;
                n2 = n;
                result.Add(n);
                index++;
            }

            return result;
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
            char[] delimeters = { ',', ' ', '.', '\t', '\n' };

            String fromReader = "";

            if (reader == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                while (reader.Peek() != -1)
                {
                    fromReader += reader.ReadLine();
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<string> splittedWords = fromReader.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();

            return splittedWords;
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
        public static IEnumerable<T> DepthTraversalTree<T>(ITreeNode<T> root)
        {
            if (root == null) throw new ArgumentNullException();

            Stack<ITreeNode<T>> stack = new Stack<ITreeNode<T>>();

            stack.Push(root);

            while (stack.Count > 0)
            {
                ITreeNode<T> current = stack.Pop();
                yield return current.Data;
                if (!(current.Children == null))
                {
                    IEnumerable<ITreeNode<T>> list = current.Children;
                    for (int i = list.Count() - 1; i >= 0; i--)
                    {
                        stack.Push(list.ElementAt(i));
                    }
                }
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
            if(root == null) throw new ArgumentNullException();

            Queue<ITreeNode<T>> nodeQueue = new Queue<ITreeNode<T>>();


            nodeQueue.Enqueue(root);
            while (nodeQueue.Count > 0)
            {
                ITreeNode<T> current = nodeQueue.Dequeue();
                yield return current.Data;
                if (!(current.Children == null))
                {
                    foreach (var currentChild in current.Children)
                    {
                        nodeQueue.Enqueue(currentChild);
                    }
                }
            }
        }




        /// <inheritdoc />
        /// <summary>
        ///   Generates all permutations of specified length from source array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">source array</param>
        /// <param name="count">permutation length</param>
        /// <returns>
        ///    All permuations of specified length
        /// </returns>
        /// <exception cref="!:System.InvalidArgumentException">count is less then 0 or greater then the source length</exception>
        /// <example>
        ///   source = { 1,2,3,4 }, count=1 =&gt; {{1},{2},{3},{4}}
        ///   source = { 1,2,3,4 }, count=2 =&gt; {{1,2},{1,3},{1,4},{2,3},{2,4},{3,4}}
        ///   source = { 1,2,3,4 }, count=3 =&gt; {{1,2,3},{1,2,4},{1,3,4},{2,3,4}}
        ///   source = { 1,2,3,4 }, count=4 =&gt; {{1,2,3,4}}
        ///   source = { 1,2,3,4 }, count=5 =&gt; ArgumentOutOfRangeException
        /// </example>

        



        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count)
        {
            if (count > source.Length || count < 0) throw new ArgumentOutOfRangeException();

            if (count == 0) return new List<T[]>(); 

            int[] combIndexes = new int[count];

            for (int i = 0; i < count; i++)
            {
                combIndexes[i] = i;
            }

            var result = new List<T[]>();

            result.Add(AddComb(combIndexes, source, count));

            while (NextComb(combIndexes, count, source.Length))
            {
                result.Add(AddComb(combIndexes, source, count));        
            }

            return result;
        }


        static T[] AddComb<T>(int[] comb, T[] source, int k)
        {
            T[] buf = new T[k];

            for (int i = 0; i < k; i++)
            {
                buf[i] = source[comb[i]];
            }

            return buf;
        }

        static bool NextComb(int[] comb, int k, int n)
        {
            int i = k - 1;
            ++comb[i];
            while ((i >= 0) && (comb[i] >= n - k + 1 + i))
            {
                try
                {
                    --i;
                    ++comb[i];
                }
                catch (Exception)
                {
                    break;
                }
            }

            if (comb[0] > n - k)
            {
                return false;
            }

            for (i = i + 1; i < k; ++i)
            {
                comb[i] = comb[i - 1] + 1;
            }

            return true;

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
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            else
            {
                var value = builder();
                dictionary.Add(key, value);
                return value;
            }
        }

    }
}
