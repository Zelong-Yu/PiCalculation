## Exercise - Value Types and Arrays- Monte Carlo Method
### Zelong Yu

A great deal of optimization is done along the way from original array-generating approach, to constant point generating, to parallel running threads, to low discrepancy sequence input in replace of `System.Random`. 

Original Approach Output - Need O(N) space for a N element array. Limit of number of points generated is memory. 
_________________________
|N        |  Estimate Pi | Diff    | Run Time(ms)|
|---------|--------------| -----   | ----|
|1|4|0.858407346410207|0.0052|
|10|3.2| 0.0584073464102071|0.0105 |
|100| 3.08| 0.061592653589793|0.0246|
|1000|3.104| 0.037592653589793| 0.1601 |
|10000|3.1516|0.0100073464102071| 1.5489 |
|100000|3.14764|0.00604734641020688| 15.043|
|1000000|3.14372|0.00212734641020695| 191.64|
|10000000| 3.1414016| 0.0001910535897931| 1662.4 |
|100000000|3.14164528 | 5.26264102069796E-05| 17964|
|130000000|3.14150833846154|  8.43151282547971E-05 |20122|
|135000000|Out of Memory|

Constant Space Approach Output - with `Enumerable.Range(1, N).Aggregate`function. Limit of number of points generated is `Int32.MaXvalue` 
_________________________
|N        |  Estimate Pi | Diff    | Run Time(ms)|
|---------|--------------| -----   | ----|
|1000000000|3.141499936|9.27175897929011E-05|161313|
|2147483647|3.1416350450095|4.23914197020814E-05 |  356876|
|2147483647|3.14155089628955| 4.17573002438054E-05| 331459|
|2147483648|ArgumentOutOfRange|

IEnumerable Approach Output - Limit of number of points generated is `Int64.Maxvalue`. Effective limit is time.  Precision convergence rate is  $O({1/\sqrt N} ))$
_________________________
|N        |  Estimate Pi | Diff    | Run Time(Minutes)|
|---------|--------------| -----   | ----|
|586000000000(5.86E11)|3.14159241105802|2.42E-07|617.58|
![Alt text](SupportingDocsAndPics/EnumerableApproach_10Hr.jpg?raw=true "IEnumerable Approach After 10 hours run")

With `Parrallel.ForEach` method the runtime can be cut to 1/4 on my 4-core I5 Surface Pro.

Low discrepancy sequence with parrallel running threads approach Output - Linear precision convergence rate $O(1/N)$
_________________________
|N        |  Estimate Pi | Diff    | Run Time(Seconds)|
|---------|--------------| -----   | ----|
|2147483637|3.14159279808287|1.44E-07|26.26|

![Alt text](SupportingDocsAndPics/ParallelSobolSequence.gif?raw=true "Parallel Running Low discrepancy sequence Approach")

Another important constant time optimization is the realization that we only need to judge if hypotenuse `Squared` is <1 to tell if a point is in unit circle. There is no need to take square root before comparision since unit circle radius 1 squared is still 1. This saves about 1/4 of run time. 

Answers
--------------
1.  Since
$$
  Ratio = \frac{number of points inside quarter unit circle} {total number of points}$$
 $$ \approx\frac{Area of quarter unit circle} {Area of 1 x 1 square} $$
 $$ =\frac{\pi*(1)^2/4}{1*1}=\frac{\pi}{4}$$
 We have $$\pi \approx 4 * Ratio.$$
2. The output becomes more and more precise. a.k.a. The difference between true $\pi$ value and our similation shrinks as number of points generated increases. However, the precision convergence rate slows down quickly. More research shows the precision convergence in $O({1/\sqrt N} ))$. To achieve linear precision convergence rate $O(1/N)$, a `low discrepancy sequence` is needed instead of pseudorandom numbers, which is the `Quasi-Monte Carlo method`.
As shown in method 6 and 7 in the program, a Sobol sequence generator is used for fast 2-d vector generation. It reduces runtime to achieve 1E-7 accuracy from 10 hours / ~5E11 points generated to about 26 seconds/ 2E9 points generated (with 4 parallel running threads. Otherwise ~100 seconds).

3. Any points number > ~1E7 requires multiple seconds of runtime. For example, 1E8 points requires 14.7 seconds run time, generating a result of 3.14167992, which has a 8.7E-5 difference from $\pi$ (4-5 digits after decimal point).

4.  Monte Carlo method is frequently used in numerical integrations. One interesting use is in machine learning especially `reinforcement learning`([# Model Free Reinforcement learning algorithms](https://medium.com/deep-math-machine-learning-ai/ch-12-1-model-free-reinforcement-learning-algorithms-monte-carlo-sarsa-q-learning-65267cb8d1b4)).



> Written with [StackEdit](https://stackedit.io/).
