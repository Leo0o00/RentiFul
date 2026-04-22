"use client";
import React, { useEffect, useRef, useState } from "react";
import { useAppSelector } from "@/state/redux";
import { useGetPropertiesQuery } from "@/state/api";
import {
  config,
  LngLatLike,
  MapOptions,
  Map,
  LngLat,
  Marker,
  Popup,
  MapStyle,
} from "@maptiler/sdk";
import "@maptiler/sdk/dist/maptiler-sdk.css";

config.apiKey = process.env.NEXT_PUBLIC_MAPTILER_API_KEY as string;

const MapComponent = () => {
  const mapContainerRef = useRef(null);
  const filters = useAppSelector((state) => state.global.filters);
  const {
    data: properties,
    isLoading,
    isError,
  } = useGetPropertiesQuery(filters);

  useEffect(() => {
    if (isLoading || isError || !properties) return;

    const center: LngLatLike = new LngLat(
      filters.coordinates.lng,
      filters.coordinates.lat
    );
    console.log({ center });

    const options: MapOptions = {
      container: mapContainerRef.current!,
      style: MapStyle.STREETS.DEFAULT,
      center,
      zoom: 9,
    };

    const map = new Map(options);

    const bbox = map.getBounds();

    console.log({ bbox });

    properties.properties.forEach((property) => {
      const marker = createPropertyMarker(property, map);
      const markerElement = marker.getElement();
      const path = markerElement.querySelector("path[fill='#3FB1CE']");
      if (path) path.setAttribute("fill", "#000000");
    });

    const resizeMap = () => {
      if (map) setTimeout(() => map.resize(), 700);
    };
    resizeMap();

    return () => map.remove();
  }, [isLoading, isError, properties, filters.coordinates]);

  if (isLoading) return <>Loading...</>;
  if (isError || !properties) return <div>Failed to fetch properties</div>;

  return (
    <div className="basis-5/12 grow relative rounded-xl">
      <div
        className="map-container rounded-xl"
        ref={mapContainerRef}
        style={{
          height: "100%",
          width: "100%",
        }}
      />
    </div>
  );
};

const createPropertyMarker = (property: PropertyListElement, map: Map) => {
  const marker = new Marker()
    .setLngLat([
      property.location.coordinates.longitude,
      property.location.coordinates.latitude,
    ])
    .setPopup(
      new Popup({ closeButton: false }).setHTML(
        `
        <div class="marker-popup">
          <div class="marker-popup-image"><img src="${property.photoUrls[0]}" alt=""/></div>
          <div>
            <a href="/search/${property.id}" target="_blank" class="marker-popup-title">${property.name}</a>
            <p class="marker-popup-price">
              $${property.pricePerMonth}
              <span class="marker-popup-price-unit"> / month</span>
            </p>
          </div>
        </div>
        `
      )
    )
    .addTo(map);
  return marker;
};

export default MapComponent;
