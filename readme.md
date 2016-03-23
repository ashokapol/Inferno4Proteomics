#Inferno for Proteomics

This is the source code for Inferno. Binary package is at http://ashokapol.github.io/Inferno/

This package contains latest release of Inferno.
Inferno can perform various downstream analyses on large scale datasets from proteomics and microarrays.

Among many features of Inferno are:
* A set of diagnostic plots (Histograms, boxplots, correlation plots, qq-plots, peptide-protein rollup plots, MA plots, PCA plots, etc).
* Log transforming.
* Rolling up to proteins (3 methods are available).
* LOESS normalization
* Linear Regression Normalization
* Mean Centering
* Median Absolute Deviation (MAD) Adjustment across datasets
* Quantile Normalization
* Principal Component Analysis
* Partial Least Squares Analysis
* ANOVA (multi-way, unbalanced, random effects)
* Heatmaps with Hierarchical and K-means cluster options

Dependencies:

Inferno depends on the following:

1. MS Windows OS with .NET 2.0 framework (http://www.microsoft.com/downloads/)
2. R Statistical Environment (http://www.r-project.org/)
3. statconnDCOM server (http://rcom.univie.ac.at/)

Inferno uses the following R packages among few others (from http://cran.r-project.org/):
* amap: Another Multidimensional Analysis Package
* car: Companion to Applied Regression
* nlme: Linear and Nonlinear Mixed Effects Models
* outliers: Tests for outliers
* fpc: Fixed point clusters, clusterwise regression and discriminant plots
* pls: Partial Least Squares Regression (PLSR) and Principal Component Regression (PCR)
* MASS: Main Package of Venables and Ripley's MASS
* impute: Imputation for microarray data
* qvalue: Q-value estimation for false discovery rate control
* e1071: Misc Functions of the Department of Statistics (e1071), TU Wien
* gplots: Various R programming tools for plotting data
* ellipse: Functions for drawing ellipses and ellipse-like confidence regions
* plotrix: Various plotting functions
* scatterplot3d: 3D Scatter Plot
* colorspace: Colorspace Manipulation
***
Developed by Ashoka Polpitiya

E-mail: ashoka.pol @ gmail.com
***
License Agreement

Inferno is licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License.  You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

All publications that utilize this software should provide the citation below:

Polpitiya AD, Qian WJ, Jaitly N, Petyuk VA, Adkins JN, Camp DG 2nd, Anderson GA, Smith RD., DAnTE: a statistical tool for quantitative analysis of -omics data. Bioinformatics. 2008 Jul 1;24(13):1556-8. (PMID: 18453552)
