"use client";

import Card from "@/components/Card";
import Header from "@/components/Header";
import Loading from "@/components/Loading";
import {
  useGetAuthUserQuery,
  useGetCurrentResidencesIdsQuery,
  useGetPropertiesQuery,
  useGetTenantQuery,
} from "@/state/api";
import React from "react";

const Favorites = () => {
  const { data: authUser } = useGetAuthUserQuery();
  const { data: tenant } = useGetTenantQuery(
    authUser?.cognitoInfo?.userId || "",
    {
      skip: !authUser?.cognitoInfo?.userId,
    }
  );

  const {
    data: favoriteProperties,
    isLoading: isFavoritePropertiesLoading,
    error,
  } = useGetPropertiesQuery(
    { favoriteIds: tenant?.favoriteProperties },
    {
      skip:
        !tenant?.favoriteProperties || tenant?.favoriteProperties.length === 0,
    }
  );

  const { data, isLoading: isCurrentResidencesIdsLoading } =
    useGetCurrentResidencesIdsQuery(authUser?.cognitoInfo?.userId ?? "", {
      skip: !authUser?.cognitoInfo?.userId,
    });

  if (isFavoritePropertiesLoading || isCurrentResidencesIdsLoading)
    return <Loading />;
  if (error) return <div>Error loading favorites</div>;

  const currentResidencesIds = data?.ownedProperties ?? [];

  return (
    <div className="dashboard-container">
      <Header
        title="Favorited Properties"
        subtitle="Browse and manage your saved property listings"
      />
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {favoriteProperties?.properties.map((property) => {
          const isOwnedProperty = currentResidencesIds.some(
            (currentResidenceId) => property.id === currentResidenceId
          );

          return (
            <Card
              key={property.id}
              property={property}
              isFavorite={true}
              onFavoriteToggle={() => {}}
              showFavoriteButton={false}
              propertyLink={
                isOwnedProperty
                  ? `/tenants/residences/${property.id}`
                  : `/search/${property.id}`
              }
            />
          );
        })}
      </div>
      {(!favoriteProperties || favoriteProperties.count === 0) && (
        <p>You don&lsquo;t have any favorited properties</p>
      )}
    </div>
  );
};

export default Favorites;
