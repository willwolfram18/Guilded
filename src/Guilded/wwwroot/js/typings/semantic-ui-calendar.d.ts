interface CalendarPopupOptions {
    position: string
}

interface CalendarOptions {
    type?: "datetime" | "date" | "time" | "month" | "year",
    popupOptions?: CalendarPopupOptions
}

interface JQuery {
    calendar(opts?: CalendarOptions): JQuery;
}