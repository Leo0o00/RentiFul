"use client";

import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import queryString from "query-string";
import { useAppSelector } from "@/state/redux";
import { usePathname, useRouter } from "next/navigation";
import { cleanParams } from "@/lib/utils";

const ReduxToUrlSync = () => {
  const router = useRouter();
  const pathname = usePathname();
  // Get current search state from Redux
  const filtersState = useAppSelector((state) => state.global.filters);

  useEffect(() => {
    // Debounce to avoid excessive URL updates (e.g., when typing)
    const debounceTimeout = setTimeout(() => {
      const cleanFilters = cleanParams(filtersState);
      const updatedSearchParams = new URLSearchParams();

      Object.entries(cleanFilters).forEach(([key, value]) => {
        updatedSearchParams.set(
          key,
          Array.isArray(value) ? value.join(",") : value.toString()
        );
      });

      // Update URL with new query params (replace: false → adds to history stack)
      router.push(`${pathname}?${updatedSearchParams.toString()}`);
    }, 300); // Adjust debounce time as needed (300ms = 3 updates/sec max)

    // Cleanup timeout on re-render/unmount
    return () => clearTimeout(debounceTimeout);
  }, [filtersState, pathname, router]); // Re-run when filtersState or navigate changes

  return null; // This component has no UI
};

export default ReduxToUrlSync;
