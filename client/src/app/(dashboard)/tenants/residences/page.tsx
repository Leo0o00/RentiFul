"use client";

import Card from "@/components/Card";
import Header from "@/components/Header";
import Loading from "@/components/Loading";
import {
  useGetAuthUserQuery,
  useGetCurrentResidencesIdsQuery,
  useGetCurrentResidencesQuery,
  useGetTenantQuery,
} from "@/state/api";
import React from "react";

const Residences = () => {
  const { data: authUser } = useGetAuthUserQuery();
  const { data: tenant } = useGetTenantQuery(
    authUser?.cognitoInfo?.userId || "",
    {
      skip: !authUser?.cognitoInfo?.userId,
    }
  );

  const {
    data,
    isLoading: isLoadingIds,
    error: errorGettingIds,
  } = useGetCurrentResidencesIdsQuery(authUser?.cognitoInfo?.userId || "", {
    skip: !authUser?.cognitoInfo?.userId,
  });

  const currentResidencesIds = data?.ownedProperties;

  const {
    data: currentResidences,
    isLoading: isLoadingResidencesData,
    error: errorGettingResidencesData,
  } = useGetCurrentResidencesQuery(
    { ids: currentResidencesIds ?? [] },
    {
      skip: !currentResidencesIds,
    }
  );

  if (isLoadingIds || isLoadingResidencesData) return <Loading />;
  if (errorGettingIds || errorGettingResidencesData)
    return <div>Error loading current residences</div>;

  return (
    <div className="dashboard-container">
      <Header
        title="Current Residences"
        subtitle="View and manage your current living spaces"
      />
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {currentResidences?.properties?.map((property) => (
          <Card
            key={property.id}
            property={property}
            isFavorite={
              tenant?.favoriteProperties.includes(property.id) || false
            }
            onFavoriteToggle={() => {}}
            showFavoriteButton={false}
            propertyLink={`/tenants/residences/${property.id}`}
          />
        ))}
      </div>
      {(!currentResidences || currentResidences.count === 0) && (
        <p>You don&lsquo;t have any current residences</p>
      )}
    </div>
  );
};

export default Residences;
