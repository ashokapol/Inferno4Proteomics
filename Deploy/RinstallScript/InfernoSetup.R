try(detach("package:rscproxy", unload = TRUE), silent = TRUE)
try(detach("package:rcom", unload = TRUE), silent = TRUE)
try(detach("package:Cairo", unload = TRUE), silent = TRUE)
try(detach("package:qvalue", unload = TRUE), silent = TRUE)

install.packages('Cairo',,'http://www.rforge.net/')
install.packages("rcom")
source("http://www.bioconductor.org/biocLite.R")
biocLite("qvalue")

Rpackages <- c('gplots','ellipse','plotrix','car','outliers','fpc','pls','scatterplot3d','MASS','impute','colorspace','nlme','qvalue','e1071','Hmisc','amap','ggplot2')
install.packages(Rpackages)

library(rcom)
installstatconnDCOM()