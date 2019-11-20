# SOFlow

--- *Original concept credited to [Ryan Hipple](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=2s)* --- 

# Requirements

Any Unity version supporting API compatibility version .NET 4.x

# Summary

The purpose of this SDK is to help bolster production of games and applications built within Unity by
taking advantage of `ScriptableObjects`. As explained in [Ryan Hipple's](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=2s) talk, `ScriptableObjects` have great potential to decouple components from
scenes and other components. This is achieved by encapsulating functionality normally contained within
`MonoBehaviour` components into `ScriptableObjects` instead. By doing this, we automatically already
remove any scene dependencies. But `ScriptableObjects` can do so much more. This SDK is an attempt to
realize that potential.

# Features

- **Data Types**

One of the foundations of the SDK. Other components in one form or another build upon what is known as
`Primitive Data Types`. Theoretically, one could create an infinite number of `Data Types` supported by
the SDK. The SDK itself provides the most commonly used `Data Types` ready to be used within projects
as illustrated below:

<p align="center"><img src="https://i.imgur.com/iLdAI2A.gif"></p>

**So what are `Data Types` exactly?**

They are containers for different types of data. The data they store is normally data stored exclusively 
within components used within scenes. This exclusivity makes it cumbersome, and at times difficult, to
interact or share the data with other components. Storing this data within containers instead allows for
far greater flexibility and provides immediate independence from scenes.

The containers come in the form of files stored within the project (`ScriptableObjects`). These containers
can then be used by other components in a variety of ways. One example is the `Data Text Setter` component,
which takes any arbitrary `Primitive Data Type` and displays the current data within the container in a
`TextMesh Pro Text` component:

<p align="center"><img src="https://i.imgur.com/9JbJqnk.gif"></p>

It is sometimes desirable to keep exclusivity of data on a per scene/component basis. `Primitive Data Types`
can be used for both cases. Each data type comes with a native ability to toggle between using data
containers or using exclusive values:

<p align="center"><img src="https://i.imgur.com/wMo2J0c.gif"></p>

NOTE: Using the `Primitive Data` root class as a variable does not support toggling between data containers
and exclusive values, as there would be no data defined as exclusive values.
