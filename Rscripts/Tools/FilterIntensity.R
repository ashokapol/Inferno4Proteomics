# Written by Ashoka D. Polpitiya
# for the Translational Genomics Research Institute (TGen, Phoenix, AZ)
# Copyright 2010, Translational Genomics Research Institute
# E-mail: ashoka@tgen.org
# Website: http://inferno4proteomics.googlecode.com
# -------------------------------------------------------------------------
#
# Licensed under the Apache License, Version 2.0; you may not use this file except
# in compliance with the License.  You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
#
# -------------------------------------------------------------------------
# Filter data based on a cutoff for missing values


intensityFilter <- function(Data, cutoff=5000, method="mean")
{
    Mode <- switch(method,
        mean = mean,
        median = median,
        minimum = min
        )
    idx <- (apply(Data, 1, Mode, na.rm=TRUE) >= cutoff)

    out <- Data[idx, ,drop=FALSE]
    return(out)
}
