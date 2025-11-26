# תכנון RESTful API - מערכת ניהול גלריה 🎨

## תיאור הפרויקט

מערכת לניהול גלריה לאומנות. באמצעות המערכת ניתן לנהל אמנים, יצירות אומנות, תערוכות ומכירות. המערכת מאפשרת מעקב אחר המלאי האומנותי, ארגון תערוכות וניהול מכירות.

---

## ישויות

| ישות | תיאור |
|------|--------|
| **Artist** | אמן המציג בגלריה |
| **Artwork** | יצירת אומנות |
| **Exhibition** | תערוכה |
| **Sale** | מכירה של יצירה |

---

## מיפוי Routes

### 🎭 Artists (אמנים)

| פעולה | Method | Route |
|-------|--------|-------|
| שליפת רשימת אמנים | GET | `/artists` |
| שליפת אמן לפי מזהה | GET | `/artists/:id` |
| הוספת אמן | POST | `/artists` |
| עדכון אמן | PUT | `/artists/:id` |
| **עדכון סטטוס אמן** | PATCH | `/artists/:id/status` |
| **שליפת כל היצירות של אמן** | GET | `/artists/:id/artworks` |

> סטטוסים אפשריים: active, inactive

---

### 🖼️ Artworks (יצירות)

| פעולה | Method | Route |
|-------|--------|-------|
| שליפת רשימת יצירות | GET | `/artworks` |
| שליפת יצירה לפי מזהה | GET | `/artworks/:id` |
| הוספת יצירה | POST | `/artworks` |
| עדכון יצירה | PUT | `/artworks/:id` |
| מחיקת יצירה | DELETE | `/artworks/:id` |
| **עדכון סטטוס יצירה** | PATCH | `/artworks/:id/status` |

> סטטוסים אפשריים: available, sold, on_loan, reserved

---

### 🏛️ Exhibitions (תערוכות)

| פעולה | Method | Route |
|-------|--------|-------|
| שליפת רשימת תערוכות | GET | `/exhibitions` |
| שליפת תערוכה לפי מזהה | GET | `/exhibitions/:id` |
| הוספת תערוכה | POST | `/exhibitions` |
| עדכון תערוכה | PUT | `/exhibitions/:id` |
| מחיקת תערוכה | DELETE | `/exhibitions/:id` |
| **שליפת יצירות בתערוכה** | GET | `/exhibitions/:id/artworks` |
| **הוספת יצירה לתערוכה** | POST | `/exhibitions/:id/artworks` |
| **הסרת יצירה מתערוכה** | DELETE | `/exhibitions/:id/artworks/:artworkId` |

---

### 💰 Sales (מכירות)

| פעולה | Method | Route |
|-------|--------|-------|
| שליפת רשימת מכירות | GET | `/sales` |
| שליפת מכירה לפי מזהה | GET | `/sales/:id` |
| הוספת מכירה (ביצוע מכירה) | POST | `/sales` |
| עדכון מכירה | PUT | `/sales/:id` |
| ביטול מכירה | DELETE | `/sales/:id` |
| **שליפת מכירות לפי טווח תאריכים** | GET | `/sales?from=2024-01-01&to=2024-12-31` |

---

## פעולות נוספות מיוחדות ⭐

| פעולה | Method | Route | תיאור |
|-------|--------|-------|--------|
| סטטיסטיקות מכירות | GET | `/sales/stats` | סה"כ מכירות, ממוצע וכו' |
| יצירות זמינות למכירה | GET | `/artworks?status=available` | סינון לפי סטטוס |
| תערוכות פעילות | GET | `/exhibitions?active=true` | תערוכות שמתקיימות כרגע |
| חיפוש יצירות | GET | `/artworks?search=שמן+על+בד` | חיפוש לפי טכניקה/שם |

---

## הערות

- כל השליפות של רשימות תומכות בפרמטרים של סינון, מיון ו-pagination
- דוגמה: `GET /artworks?artist=5&sort=price&order=desc&page=1&limit=10`
