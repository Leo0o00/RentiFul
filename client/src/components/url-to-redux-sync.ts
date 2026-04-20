"use client";

import { useEffect } from "react";
import { useDispatch } from "react-redux";
import queryString from "query-string";
import { setFilters } from "@/state";
import { useSearchParams } from "next/navigation";

const UrlToReduxSync = () => {
  const dispatch = useDispatch();
  const searchParams = useSearchParams();

  useEffect(() => {
    // Parse query parameters from URL
    const searchParamsEntries = searchParams.toString();
    const parsedParams = queryString.parse(searchParams.toString(), {
      arrayFormat: "comma", // Match arrayFormat from ReduxToUrlSync
      parseNumbers: true, // Convert "page=2" → page: 2 (instead of string)
    });
    console.log({ searchParamsEntries });
    console.log({ parsedParams });

    // Dispatch action to update Redux state
    dispatch(setFilters(parsedParams));
  }, [searchParams, dispatch]); // Re-run when URL search params change

  return null; // No UI
};

export default UrlToReduxSync;
