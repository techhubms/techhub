---
layout: "post"
title: "Iterate Controls in ASP.NET ListView and Conditionally Remove Panels from Placeholder"
description: "The author seeks help iterating over controls within an ASP.NET ListView to remove a specific Panel from a Placeholder if a database condition is met. However, all instances of the Panel are being removed regardless of the condition. The post includes sample code for context."
author: "canalso"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/web-development/aspnet-c-iterate-control-in-listview-and-remove-a-panel-from/m-p/4413198#M658"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet"
date: 2025-05-13 03:59:20 +00:00
permalink: "/2025-05-13-Iterate-Controls-in-ASPNET-ListView-and-Conditionally-Remove-Panels-from-Placeholder.html"
categories: ["Coding"]
tags: ["ASP.NET", "C#", "Coding", "Community", "Conditional Logic", "Control Iteration", "DataTable", "ItemDataBound", "ListView", "Panel", "Placeholder", "Webforms"]
tags_normalized: ["aspdotnet", "csharp", "coding", "community", "conditional logic", "control iteration", "datatable", "itemdatabound", "listview", "panel", "placeholder", "webforms"]
---

In this community post, canalso describes an issue with removing a Panel from a Placeholder within an ASP.NET ListView using C#, seeking advice on how to perform this task based on a row condition.<!--excerpt_end-->

## Issue Overview

The author describes a scenario in an ASP.NET WebForms application using C# where they want to conditionally remove a `Panel` (named `panel01`) from a `Placeholder` inside each row of a `ListView`. The condition is based on a database field: if `BidEndWithError` (from the data row) equals `'no'`, then `panel01` should be removed for that row.

However, the author is facing a problem where **all** instances of `panel01` are being removed from all rows, not simply the ones that match the condition in the DataTable.

## Sample Code (Simplified)

```csharp
protected void lv01_ItemDataBound(object sender, ListViewItemEventArgs e)
{
    if (e.Item.ItemType == ListViewItemType.DataItem)
    {
        string constr = ConfigurationManager.ConnectionStrings["bidsystemdb"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            DataTable dt = new DataTable();
            string strSQL = "Select * from BidTable2";
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            adpt.Fill(dt);
            
            for (int ij = 0; ij < e.Item.Controls.Count; ij++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["BidEndWithError"].ToString() == "no")
                    {
                        PlaceHolder _phLabel = e.Item.Controls[ij].FindControl("phLabel") as PlaceHolder;
                        Panel _pnlLabel = e.Item.Controls[ij].FindControl("panel01") as Panel;
                        _phLabel.Controls.Remove(_pnlLabel);
                        // ... other update code
                    }
                }
            }
        }
    }
}
```

## Analysis

- The code loops through all controls (`ij`) for **each item** (row) in the ListView.
- Then, for each database row (`i`), it checks if `BidEndWithError` is `'no'` and tries to remove `panel01` from `phLabel`.
- This double loop causes the removal logic to run multiple times, resulting in **all panels being removed**, not just those matching the specific data item.

## Correct Approach

- In the `ItemDataBound` event, you generally work with one row/item at a time. You do **not** need to fetch all rows from the database again or loop.
- The ListView's `DataItem` property contains the data for **that one row**. Use this to determine whether to remove the panel.

**Suggested Solution:**

```csharp
protected void lv01_ItemDataBound(object sender, ListViewItemEventArgs e)
{
    if (e.Item.ItemType == ListViewItemType.DataItem)
    {
        DataRowView rowView = e.Item.DataItem as DataRowView;
        if (rowView != null && rowView["BidEndWithError"].ToString() == "no")
        {
            PlaceHolder phLabel = e.Item.FindControl("phLabel") as PlaceHolder;
            Panel pnlLabel = e.Item.FindControl("panel01") as Panel;
            if (phLabel != null && pnlLabel != null)
            {
                phLabel.Controls.Remove(pnlLabel);
            }
        }
    }
}
```

**Key Points:**

- Use `e.Item.DataItem` to get the data for the current row.
- No need to query the database or iterate over rows inside `ItemDataBound`.
- Only remove the panel if the current data item meets the condition.

## Conclusion

By adjusting your code to operate on the current `DataItem` during binding, you can correctly target the panel in the relevant row rather than applying the logic to all rows. This results in only the intended panel being removed when the condition is met.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/web-development/aspnet-c-iterate-control-in-listview-and-remove-a-panel-from/m-p/4413198#M658)
