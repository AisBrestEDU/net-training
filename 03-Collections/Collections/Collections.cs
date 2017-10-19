using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Collections.Tasks
{

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
			if (count < 0)
				throw new ArgumentException();

			int temp;
			int previous = 0, current = 1;
			
			for (int i = 0; i < count; i++)
			{
				yield return current;
				temp = current;
				current += previous;
				previous = temp;
				
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
			char[] delimeters = { ',', ' ', '.', '\t', '\n' };

			if (reader == null)		throw new ArgumentNullException();
			
			var line = reader.ReadLine();
			while (line != null)
			{
				var words = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
				foreach (var item in words)
				{
					yield return item;
				}
				line = reader.ReadLine();
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

			if (root == null)
				throw new ArgumentNullException();

			var deepFirst = new Stack<ITreeNode<T>>();
			deepFirst.Push(root);

			while (deepFirst.Count > 0)
			{
				var lastElement = deepFirst.Pop();
				yield return lastElement.Data;

				if (lastElement.Children != null)
				{
					var arrayChildren = lastElement.Children.ToArray().Reverse();
					foreach (var item in arrayChildren)
					{
						deepFirst.Push(item);
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
			// TODO : Implement the tree width traversal algorithm

			if (root == null)
				throw new ArgumentNullException();

			var widthFirst = new Queue<ITreeNode<T>>();
			widthFirst.Enqueue(root);

			while (widthFirst.Count > 0)
			{
				var current = widthFirst.Dequeue();
				yield return current.Data;

				if (current.Children != null)
				{
					foreach (var item in current.Children)
					{
						widthFirst.Enqueue(item);
					}
				}
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
		public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count)
		{
			if (count > source.Length || count < 0)
				throw new ArgumentOutOfRangeException();
			if (count == 0)
				return Enumerable.Empty<T[]>();

			var list = new List<T[]>();
			AddPermutationsToList(new Stack<T>(), 0);
			return list;

			void AddPermutationsToList(Stack<T> Permutation, int currentIndex)
			{
				if (count - Permutation.Count == 1)
				{
					for (int i = currentIndex; i < source.Length; i++)
					{
						Permutation.Push(source[i]);
						list.Add(Permutation.Reverse().ToArray());
						Permutation.Pop();
					}
				}
				else
				{
					for (int i = currentIndex; i <= source.Length - (count - Permutation.Count); i++)
					{
						Permutation.Push(source[i]);
						AddPermutationsToList(Permutation, i + 1);
						Permutation.Pop();
					}
				}
			}
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
				return dictionary[key];
			else
			{
				dictionary.Add(key, builder());
				return dictionary[key];
			}
        }

    }
}
