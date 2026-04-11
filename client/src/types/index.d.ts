import { LucideIcon } from "lucide-react";
import { AuthUser } from "aws-amplify/auth";
import { Manager, Tenant, Property, Application } from "./prismaTypes";
import { MotionProps as OriginalMotionProps } from "framer-motion";

declare module "framer-motion" {
  interface MotionProps extends OriginalMotionProps {
    className?: string;
  }
}

declare global {
  enum AmenityEnum {
    WasherDryer = "WasherDryer",
    AirConditioning = "AirConditioning",
    Dishwasher = "Dishwasher",
    HighSpeedInternet = "HighSpeedInternet",
    HardwoodFloors = "HardwoodFloors",
    WalkInClosets = "WalkInClosets",
    Microwave = "Microwave",
    Refrigerator = "Refrigerator",
    Pool = "Pool",
    Gym = "Gym",
    Parking = "Parking",
    PetsAllowed = "PetsAllowed",
    WiFi = "WiFi",
  }

  enum HighlightEnum {
    HighSpeedInternetAccess = "HighSpeedInternetAccess",
    WasherDryer = "WasherDryer",
    AirConditioning = "AirConditioning",
    Heating = "Heating",
    SmokeFree = "SmokeFree",
    CableReady = "CableReady",
    SatelliteTV = "SatelliteTV",
    DoubleVanities = "DoubleVanities",
    TubShower = "TubShower",
    Intercom = "Intercom",
    SprinklerSystem = "SprinklerSystem",
    RecentlyRenovated = "RecentlyRenovated",
    CloseToTransit = "CloseToTransit",
    GreatView = "GreatView",
    QuietNeighborhood = "QuietNeighborhood",
  }

  enum PropertyTypeEnum {
    Rooms = "Rooms",
    Tinyhouse = "Tinyhouse",
    Apartment = "Apartment",
    Villa = "Villa",
    Townhouse = "Townhouse",
    Cottage = "Cottage",
  }

  interface SidebarLinkProps {
    href: string;
    icon: LucideIcon;
    label: string;
  }

  interface PropertyOverviewProps {
    propertyId: string;
  }

  interface ApplicationModalProps {
    isOpen: boolean;
    onClose: () => void;
    propertyId: string;
  }

  interface ContactWidgetProps {
    managerId: string;
    onOpenModal: () => void;
  }

  interface ImagePreviewsProps {
    images: string[];
  }

  interface PropertyDetailsProps {
    propertyId: string;
  }

  interface PropertyOverviewProps {
    propertyId: string;
  }

  interface PropertyLocationProps {
    propertyId: string;
  }

  interface ApplicationCardProps {
    application: Application;
    userType: "manager" | "renter";
    children: React.ReactNode;
  }

  interface CardProps {
    property: PropertyListElement;
    isFavorite: boolean;
    onFavoriteToggle: () => void;
    showFavoriteButton?: boolean;
    propertyLink?: string;
  }

  interface CardCompactProps {
    property: Property;
    isFavorite: boolean;
    onFavoriteToggle: () => void;
    showFavoriteButton?: boolean;
    propertyLink?: string;
  }

  interface HeaderProps {
    title: string;
    subtitle: string;
  }

  interface NavbarProps {
    isDashboard: boolean;
  }

  interface AppSidebarProps {
    userType: "manager" | "tenant";
  }

  interface SettingsFormProps {
    initialData: SettingsFormData;
    onSubmit: (data: SettingsFormData) => Promise<void>;
    userType: "manager" | "tenant";
  }

  interface User {
    cognitoInfo: AuthUser;
    userInfo: Tenant | Manager;
    userRole: JsonObject | JsonPrimitive | JsonArray;
  }

  interface Tenant {
    cognitoId: string;
    name: string;
    email: string;
    phoneNumber: string;
    createdAt: Date;
    updatedAt: Date;
    favoriteProperties: string[];
  }
  interface Manager {
    cognitoId: string;
    name: string;
    email: string;
    phoneNumber: string;
    createdAt: Date;
    updatedAt?: Date;
  }

  interface PropertyList {
    count: number;
    properties: PropertyListElement[];
  }

  interface PropertyListElement {
    id: string;
    name: string;
    photoUrls: string[];
    isPetsAllowed: boolean;
    isParkingIncluded: boolean;
    location: Location;
    averageRating: number;
    numberOfReviews: number;
    pricePerMonth: number;
    beds: number;
    baths: number;
    squareFeet: number;
  }

  interface Location {
    address: string;
    city: string;
    coordinates: Coordinates;
  }

  interface Coordinates {
    latitude: number;
    longitude: number;
  }

  interface PropertyDetails {
    id: string;
    name: string;
    description: string;
    pricePerMonth: number;
    securityDeposit: number;
    applicationFee: number;
    amenities: AmenityEnum[];
    highlights: HighlightEnum[];
    isPetsAllowed: boolean;
    isParkingIncluded: boolean;
    beds: number;
    baths: number;
    squareFeet: number;
    averageRating: number;
    numberOfReviews: number;
    managerId: string;

    location: LocationDetails;
    photoUrls?: string[];
  }

  interface LocationDetails {
    address: string;
    country: string;
    state: string;
    city: string;
    coordinates: Coordinates;
  }

  interface CreateApplicationRequest {
    applicationDate: string;
    status: string;
    propertyId: string;
    tenantCognitoId: string;
    message?: string;
  }

  interface ApplicationsList {
    count: number;
    applications: Application[];
  }

  interface Application {
    id: string;
    submitedAt: string;
    status: string;
    property?: PropertyInfoDto;
    tenant?: TenantInfoDto;
    manager?: ManagerInfoDto;
    lease?: LeaseInfoDto;
  }

  interface PropertyInfoDto {
    id: string;
    name: string;
    pricePerMonth: number;
    photoUrl: string;
    location: LocationDto;
  }

  interface LocationDto {
    city: string;
    country: string;
  }
  interface TenantInfoDto {
    congitoId: string;
    name: string;
    phoneNumber: string;
    email: string;
  }
  interface ManagerInfoDto {
    congitoId: string;
    name: string;
    phoneNumber: string;
    email: string;
  }
  interface LeaseInfoDto {
    id: string;
    startDate: string;
    endDate: string;
  }

  interface LeasesList {
    count: number;
    leases: LeasesListElement[];
  }

  interface LeasesListElement {
    id: string;
    startDate: string;
    endDate: string;
    rent: number;
    deposit: number;
    propertyId: string;
    tenantId: string;
    createdAt: string;
    updatedAt?: string;
  }
  interface PropertyLeasesList {
    count: number;
    leases: PropertyLeasesListElement[];
  }

  interface PropertyLeasesListElement {
    id: string;
    startDate: string;
    endDate: string;
    rent: number;
    deposit: number;
    tenant: TenantInfoDto;
    createdAt: string;
    updatedAt?: string;
  }
}

export {};
