-- A release date set by an admin was stored as midnight Brussels time: an epoch that falls in the evening of the day before in UTC.
-- The features page and the admin read and write the date as midnight UTC of the date, like the dates of the timeline events.
-- This moves the stored dates to midnight UTC of the Brussels date. Dates that are already midnight UTC do not change.
UPDATE product_features
SET release_date = EXTRACT(EPOCH FROM (to_timestamp(release_date) AT TIME ZONE 'Europe/Brussels')::date)::bigint
WHERE release_date IS NOT NULL;
