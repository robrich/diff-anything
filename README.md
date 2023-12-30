Diff Anything
=============

> Diff any two C# objects of the same type

[![Build](https://github.com/robrich/diff-anything/actions/workflows/build.yaml/badge.svg)](https://github.com/robrich/diff-anything/actions/workflows/build.yaml)

I was asked, "How would you diff all the properties of two objects in C#?"  Here is my answer.  It uses reflection and recursion.


Features
--------

- Can diff simple POCO objects
- Can diff objects nested in objects
- Can diff arrays and array properties on objects
- Can diff dictionaries and dictionary properties on objects

The magic happens in `DiffAnything/Differ.cs`. Look at `DiffAnything.Tests` for examples.


Caveats
-------

This is a quick doodle.  For a more battle-tested solution look at https://github.com/replaysMike/AnyDiff and for more methodologies of how to solve this problem look at https://beribey.medium.com/deep-compare-2-object-in-c-ff1191346736


License
-------

MIT
