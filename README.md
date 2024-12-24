# **Enemy Avoidance System - README**

Link to game:
https://gman17.itch.io/week-62

## **Overview**  
This system makes enemies move **away from the player** when inside a specified **radius**. Movement is restricted to **allowed tiles** on the **tilemap**, and enemies prioritize avoidance points based on their **relative position** (left or right) to the **player**.

---

## **Features**  
- **Dynamic Avoidance:** Enemies avoid the player based on their **position**.  
- **Tile Validation:** Moves only on **allowed tiles**.  
- **Configurable Settings:** Adjust radius, speed, angles, and logs in the **Inspector**.  
- **Debugging Tools:** Enable logs for **movement**, **tile checks**, and **detection**.

---

## **Scripts**

### **EnemyAvoidance.cs (Player Script)**  
- **Purpose:** Detects enemies and calculates avoidance points.  
- **Key Settings:**
  - **avoidRadius:** Avoidance detection radius.  
  - **angleStep:** Step size for rotating to find valid tiles.  
  - **maxAttempts:** Maximum angles to test for valid points.  
  - **logAvoidance:** Toggle logs in the **Inspector**.

### **EnemyMover.cs (Enemy Script)**  
- **Purpose:** Moves enemies toward the target avoidance point.  
- **Key Settings:**
  - **speed:** Movement speed toward the target.  
  - **stoppingDistance:** Minimum distance to stop at the target.  
  - **logMovement:** Toggle logs in the **Inspector**.

---

## **How It Works**  
1. **Enemy Detection:**  
   - Finds enemies within the **radius** using **OverlapCircleAll**.  
2. **Avoidance Point Calculation:**  
   - Determines if the **enemy is on the left or right** of the **player**.  
   - Searches for the **first valid tile** by rotating outward in steps.  
3. **Movement:**  
   - Moves **smoothly** to the target while **validating tiles**.

---

## **Setup Instructions**  
1. Attach **EnemyAvoidance.cs** to the **Player**.  
2. Attach **EnemyMover.cs** to each **Enemy**.  
3. Assign **Tilemap**, **Allowed Tiles**, and **Enemy Layer** in the **Inspector**.  
4. Adjust settings like **radius**, **angle step**, and **speed** as needed.

---
